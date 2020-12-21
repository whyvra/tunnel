using System.Collections.Generic;
using Whyvra.Tunnel.Common.Models;

namespace Whyvra.Tunnel.Presentation.ViewModels
{
    public class ClientViewModel : IClientViewModel<CreateClientDto>
    {
        public CreateClientDto Client { get; set; } = new CreateClientDto();

        public IEnumerable<string> AllowedIps { get; set; } = new List<string>();
    }
}