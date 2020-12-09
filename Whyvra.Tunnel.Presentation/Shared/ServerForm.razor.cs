using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Whyvra.Blazor.Forms;
using Whyvra.Blazor.Forms.Renderer;
using Whyvra.Tunnel.Common.Models;
using Whyvra.Tunnel.Presentation.Components;
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
        public BulmaForm<ServerViewModel> Form { get; set; }
        public FormModel<ServerViewModel> FormModel { get; set; }
        public FormViewMode FormViewMode { get; set; } = FormViewMode.Readonly;
        public bool IsLoading { get; set; }
        public bool IsNotificationVisible { get; set; }
        public NotificationDto Notification { get; set; }
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

            if (!result.IsValid)
            {
                Form.Validate();
                Notification = new NotificationDto
                {
                    Icon = "exclamation-triangle",
                    IconCss = "has-text-warning",
                    Message = "Please fill the required fields in the form below.",
                    Severity = "danger"
                };
                IsNotificationVisible = true;
                return;
            }

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
                    await NetworkAddressService.ProcessNetworkAddresses(ServerService, id, model.DefaultAllowedRange, Enumerable.Empty<string>());
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
            await NetworkAddressService.ProcessNetworkAddresses(ServerService, serverId, viewModel.DefaultAllowedRange, original.DefaultAllowedRange);

            return wasUpdated;
        }

        protected async Task ProcessDelete()
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