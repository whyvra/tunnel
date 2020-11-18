using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Whyvra.Blazor.Forms;
using Whyvra.Tunnel.Common.Commands;
using Whyvra.Tunnel.Common.Models;
using Whyvra.Tunnel.Presentation.Services;
using Whyvra.Tunnel.Presentation.ViewModels;

namespace Whyvra.Tunnel.Presentation.Shared
{
    public partial class ServerForm : ComponentBase
    {
        // Private variables
        private string _copy;
        private ServerDto _server;

        // Properties shared with component
        public FormModel<ServerViewModel> FormModel { get; set; }
        public FormViewMode FormViewMode { get; set; } = FormViewMode.Readonly;
        public bool IsLoading { get; set; }
        public bool ShowDeleteConfirm { get; set; }

        // Injectable properties
        [Inject]
        protected IValidator<ServerViewModel> Validator { get; set;}

        [Inject]
        protected NetworkAddressService NetworkAddressService { get; set; }

        [Inject]
        protected ServerService ServerService { get; set; }

        // Razor component parameters
        [Parameter]
        public int? ServerId { get; set; }

        [Parameter]
        public EventCallback<bool> CloseAction { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var fb = new FormBuilder<ServerViewModel>();

            fb
                .Input(x => x.Server.Name).WithIcon("server")
                .TextArea(x => x.Server.Description, rows: 3)
                .Input(x => x.Server.AssignedRange).WithText("Assigned IP Range").WithIcon("network-wired")
                .TagsInput(x => x.DefaultAllowedRange).WithDefaultEmptyValue("No IP range has been assigned")
                .Input(x => x.Server.Dns).WithText("DNS").WithIcon("address-book")
                .Input(x => x.Server.Endpoint).WithIcon("globe")
                .Input(x => x.Server.PublicKey).WithIcon("key");

            if (ServerId.HasValue)
            {
                // Query backend 
                _server = await ServerService.Get(ServerId.Value);
                // Store a copy for dirty checks
                _copy = JsonSerializer.Serialize(_server);

                // Instantiate our view model
                var viewModel = new ServerViewModel
                {
                    Server = new CreateUpdateServerDto
                    {
                        Name = _server.Name,
                        Description = _server.Description,
                        AssignedRange = _server.AssignedRange,
                        Dns = _server.Dns,
                        Endpoint = _server.Endpoint,
                        PublicKey = _server.PublicKey
                    },
                    DefaultAllowedRange = _server.DefaultAllowedRange
                };

                fb.WithModel(viewModel);
            }
            else
            {
                FormViewMode = FormViewMode.Blank;
            }

            FormModel = fb.Build();
        }

        protected async Task SaveChanges()
        {
            // Check if model is valid
            var model = FormModel.DataModel;
            var result = Validator.Validate(model);

            if (result.IsValid)
            {
                // Start loading screen
                IsLoading = true;
                _server = null;
                var wasUpdated = false;

                if (FormViewMode == FormViewMode.Edit && ServerId.HasValue)
                {
                    wasUpdated = await Update(model, ServerId.Value);
                }
                else if (FormViewMode == FormViewMode.Blank)
                {
                    var id = await ServerService.CreateNew(model.Server);
                    wasUpdated = true;
                    await ProcessNetWorkAddresses(id, model.DefaultAllowedRange, Enumerable.Empty<string>());
                }

                // Trigger modal close
                //IsLoading = false;
                await CloseAction.InvokeAsync(wasUpdated);
            }
        }

        private async Task<bool> Update(ServerViewModel viewModel, int serverId)
        {
            // Load stored copy for dirty check
            var original = JsonSerializer.Deserialize<ServerDto>(_copy);
            var wasUpdated = false;

            if(IsDirty(viewModel.Server, original))
            {
                // Update record if properties have changed
                await ServerService.Update(serverId, viewModel.Server);
                wasUpdated = true;
            }

            // Process network address in DefaultAllowedRange
            await ProcessNetWorkAddresses(serverId, viewModel.DefaultAllowedRange, original.DefaultAllowedRange);

            return wasUpdated;
        }

        private async Task ProcessNetWorkAddresses(int serverId, IEnumerable<string> newRange, IEnumerable<string> oldRange)
        {
            if (!newRange.SequenceEqual(oldRange))
            {
                // Figure out what needs to be added or removed
                var toAdd = newRange.Except(oldRange);
                var toRemove = oldRange.Except(newRange);

                // Get all existing network addresses
                var addresses = await NetworkAddressService.GetAll();

                // Process address to add
                foreach(var addr in toAdd)
                {
                    if (addresses.Any(x => x.Address.Equals(addr)))
                    {
                        // Address already exists so get it's ID and just add it
                        var netId = addresses.Single(x => x.Address.Equals(addr)).Id;
                        await ServerService.AddToAllowedRange(serverId, netId);
                    }
                    else
                    {
                        // Address doesn't exist so create it first and then add it
                        var command = new CreateNetworkAddressCommand { Address = addr };
                        var id = await NetworkAddressService.CreateNew(command);
                        await ServerService.AddToAllowedRange(serverId, id);
                    }
                }

                // Process address to remove from server
                foreach (var addr in toRemove)
                {
                    var netId = addresses.Single(x => x.Address.Equals(addr)).Id;
                    await ServerService.RemoveFromAllowedRange(serverId, netId);
                }
            }
        }

        private async Task ProcessDelete()
        {
            IsLoading = true;
            ShowDeleteConfirm = false;

            if (ServerId.HasValue)
            {
                await ServerService.Delete(ServerId.Value);
            }

            await CloseAction.InvokeAsync(true);
        }

        private bool IsDirty(CreateUpdateServerDto server, ServerDto original)
        {
            return !(server.Name.Equals(original.Name)
                && server.Description.Equals(original.Description)
                && server.AssignedRange.Equals(original.AssignedRange)
                && server.Dns.Equals(original.Dns)
                && server.Endpoint.Equals(original.Endpoint)
                && server.PublicKey.Equals(original.PublicKey));
        }
    }
}