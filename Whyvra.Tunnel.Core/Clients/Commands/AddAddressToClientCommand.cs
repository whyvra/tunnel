using MediatR;

namespace Whyvra.Tunnel.Core.Clients.Commands
{
    public class AddAddressToClientCommand : IRequest<int>
    {
        public int ClientId { get; set; }

        public int NetworkAddressId { get; set; }
    }
}