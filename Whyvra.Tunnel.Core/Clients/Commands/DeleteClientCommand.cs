using MediatR;

namespace Whyvra.Tunnel.Core.Clients.Commands
{
    public class DeleteClientCommand : IRequest
    {
        public int Id { get; set; }
    }
}