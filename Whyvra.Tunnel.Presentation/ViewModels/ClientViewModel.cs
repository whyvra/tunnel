using System.Collections.Generic;
using Whyvra.Tunnel.Common.Models;

namespace Whyvra.Tunnel.Presentation.ViewModels
{
    public class ClientViewModel
    {
        public CreateUpdateClientDto Client { get; set; } = new CreateUpdateClientDto();

        public IEnumerable<string> AllowedIps { get; set; } = new List<string>();
    }
}