using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Whyvra.Blazor.Forms;
using Whyvra.Tunnel.Common.Models;
using Whyvra.Tunnel.Presentation.ViewModels;

namespace Whyvra.Tunnel.Presentation.Shared
{
    public partial class ClientForm : ComponentBase
    {
        private FormModel<ClientViewModel> _formModel;

        [Parameter]
        public EventCallback<bool> CloseAction { get; set; }

        [Parameter]
        public ServerDto ServerDto { get; set; }

        protected override Task OnInitializedAsync()
        {
            var viewModel = new ClientViewModel
            {
                AllowedIps = ServerDto.DefaultAllowedRange
            };

            _formModel = new FormBuilder<ClientViewModel>()
                .Input(x => x.Client.Name).WithIcon("user")
                .TextArea(x => x.Client.Description)
                .Input(x => x.Client.AssignedIp).WithIcon("network-wired").WithText("Assigned IP")
                .Checkbox(x => x.Client.IsIpAutoGenerated).HideOnCheck(x => x.Name.Equals("AssignedIp")).WithText("Use next available IP address")
                .TagsInput(x => x.AllowedIps).WithDefaultEmptyValue("No IPs allowed").WithText("Allowed IPs")
                .WithModel(viewModel)
                .Build();

            return Task.CompletedTask;
        }

        protected async Task HandleClose(bool wasUpdated = false)
        {
            await CloseAction.InvokeAsync(wasUpdated);
        }
    }
}