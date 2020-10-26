using MediatR;
using Whyvra.Tunnel.Common.Models;

namespace Whyvra.Tunnel.Core.Servers.Queries
{
    public class GetServerByIdQuery : IRequest<ServerDto>
    {
        public int Id { get; set; }
    }
}