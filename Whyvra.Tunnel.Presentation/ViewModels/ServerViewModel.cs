using System.Collections.Generic;
using Whyvra.Tunnel.Common.Models;

namespace Whyvra.Tunnel.Presentation.ViewModels
{
    public class ServerViewModel
    {
        public CreateUpdateServerDto Server { get; set; } = new CreateUpdateServerDto();

        public IEnumerable<string> DefaultAllowedRange { get; set; } = new List<string>();
    }
}