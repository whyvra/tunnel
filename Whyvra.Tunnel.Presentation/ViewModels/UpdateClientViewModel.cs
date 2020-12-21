using System.Collections.Generic;
using Whyvra.Tunnel.Common.Models;

namespace Whyvra.Tunnel.Presentation.ViewModels
{
    public class UpdateClientViewModel : IClientViewModel<UpdateClientDto>
    {
        public UpdateClientDto Client { get; set; } = new UpdateClientDto();

        public IEnumerable<string> AllowedIps { get; set; } = new List<string>();
    }
}