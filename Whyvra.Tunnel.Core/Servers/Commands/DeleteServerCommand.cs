using MediatR;

namespace Whyvra.Tunnel.Core.Servers.Commands
{
    public class DeleteServerCommand : IRequest
    {
        public int Id { get; set; }
    }
}