using MediatR;

namespace Whyvra.Tunnel.Core.Clients.Commands
{
    public class RemoveAddressFromClientCommand : IRequest
    {
        public int ClientId { get; set; }

        public int NetworkAddressId { get; set; }
    }
}