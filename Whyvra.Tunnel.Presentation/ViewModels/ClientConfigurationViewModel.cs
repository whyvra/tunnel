using Whyvra.Tunnel.Common.Models;

namespace Whyvra.Tunnel.Presentation.ViewModels
{
    public class ClientConfigurationViewModel
    {
        public ClientDto Client { get; set; }

        public ServerDto Server { get; set; }

        public string PrivateKey { get; set; }
    }
}