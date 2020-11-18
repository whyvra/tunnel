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

        // Injectable properties
        [Inject]
        protected IValidator<ServerViewModel> Validator { get; set;}

        [Inject]
        protected NetworkAddressService NetworkAddressService { get; set; }

        [Inject]
        protected ServerService ServerService { get; set; }

        // Razor component parameters
        [Parameter]
        public int ServerId { get; set; }

        [Parameter]
        public EventCallback<bool> CloseAction { get; set; }

        protected override async Task OnInitializedAsync()
        {
            // Query backend 
            _server = await ServerService.Get(ServerId);
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

            var fb = new FormBuilder<ServerViewModel>();

            FormModel = fb
                .Input(x => x.Server.Name).WithIcon("server")
                .TextArea(x => x.Server.Description, rows: 3)
                .Input(x => x.Server.AssignedRange).WithText("Assigned IP Range").WithIcon("network-wired")
                .TagsInput(x => x.DefaultAllowedRange).WithDefaultEmptyValue("No IP range has been assigned")
                .Input(x => x.Server.Dns).WithText("DNS").WithIcon("address-book")
                .Input(x => x.Server.Endpoint).WithIcon("globe")
                .Input(x => x.Server.PublicKey).WithIcon("key")
                .WithModel(viewModel)
                .Build();
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

                var wasUpdated = await Update(model);

                // Stop loading and trigger modal close
                IsLoading = false;
                await CloseAction.InvokeAsync(wasUpdated);
            }
        }

        private async Task<bool> Update(ServerViewModel viewModel)
        {
            // Load stored copy for dirty check
            var original = JsonSerializer.Deserialize<ServerDto>(_copy);
            var wasUpdated = false;

            if(IsDirty(viewModel.Server, original))
            {
                // Update record if properties have changed
                await ServerService.Update(ServerId, viewModel.Server);
                wasUpdated = true;
            }

            // Process network address in DefaultAllowedRange
            await ProcessNetWorkAddresses(viewModel, original);

            return wasUpdated;
        }

        private async Task ProcessNetWorkAddresses(ServerViewModel viewModel, ServerDto original)
        {
            if (!viewModel.DefaultAllowedRange.SequenceEqual(original.DefaultAllowedRange))
            {
                // Figure out what needs to be added or removed
                var toAdd = viewModel.DefaultAllowedRange.Except(original.DefaultAllowedRange);
                var toRemove = original.DefaultAllowedRange.Except(viewModel.DefaultAllowedRange);

                // Get all existing network addresses
                var addresses = await NetworkAddressService.GetAll();

                // Process address to add
                foreach(var addr in toAdd)
                {
                    if (addresses.Any(x => x.Address.Equals(addr)))
                    {
                        // Address already exists so get it's ID and just add it
                        var netId = addresses.Single(x => x.Address.Equals(addr)).Id;
                        await ServerService.AddToAllowedRange(ServerId, netId);
                    }
                    else
                    {
                        // Address doesn't exist so create it first and then add it
                        var command = new CreateNetworkAddressCommand { Address = addr };
                        var id = await NetworkAddressService.CreateNew(command);
                        await ServerService.AddToAllowedRange(ServerId, id);
                    }
                }

                // Process address to remove from server
                foreach (var addr in toRemove)
                {
                    var netId = addresses.Single(x => x.Address.Equals(addr)).Id;
                    await ServerService.RemoveFromAllowedRange(ServerId, netId);
                }
            }
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