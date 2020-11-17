using MediatR;

namespace Whyvra.Tunnel.Core.Servers.Commands
{
    public class AddAddressToServerCommand : IRequest<int>
    {
        public int NetworkAddressId { get; set; }

        public int ServerId { get; set; }
    }
}