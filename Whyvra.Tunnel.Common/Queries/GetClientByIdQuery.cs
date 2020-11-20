using MediatR;
using Whyvra.Tunnel.Common.Models;

namespace Whyvra.Tunnel.Common.Queries
{
    public class GetClientByIdQuery : IRequest<ClientDto>
    {
        public int Id { get; set; }
    }
}