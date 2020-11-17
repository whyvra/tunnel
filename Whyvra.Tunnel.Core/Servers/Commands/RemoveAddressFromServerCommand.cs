using MediatR;

namespace Whyvra.Tunnel.Core.Servers.Commands
{
    public class RemoveAddressFromServerCommand : IRequest
    {
        public int NetworkAddressId { get; set; }

        public int ServerId { get; set; }
    }
}