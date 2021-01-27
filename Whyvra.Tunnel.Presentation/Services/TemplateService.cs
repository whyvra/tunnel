using System.Collections.Generic;
using Blazorme;
using Whyvra.Tunnel.Common.Models;
using Whyvra.Tunnel.Presentation.Templates;
using Whyvra.Tunnel.Presentation.ViewModels;

namespace Whyvra.Tunnel.Presentation.Services
{
    public class TemplateService
    {
        private readonly TestHost _host;

        public TemplateService(TestHost host)
        {
            _host = host;
        }

        public string RenderClientConfiguration(ClientConfigurationViewModel viewModel)
        {
            var parameters = new Dictionary<string, object>()
            {
                { nameof(ClientConfiguration.Model), viewModel }
            };
            var component = _host.AddComponent<ClientConfiguration>(parameters);

            return component.GetMarkup();
        }

        public string RenderServerConfiguration(ServerDto server, IEnumerable<ClientDto> clients)
        {
            var parameters = new Dictionary<string, object>()
            {
                { nameof(ServerConfiguration.Server), server },
                { nameof(ServerConfiguration.Clients), clients }
            };
            var component = _host.AddComponent<ServerConfiguration>(parameters);
            return component.GetMarkup().Trim();
        }
    }
}