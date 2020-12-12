using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Whyvra.Tunnel.Common.Models;
using Whyvra.Tunnel.Data;

namespace Whyvra.Tunnel.Core.Servers.Queries
{
    public class GetRevokedClientsForServerQueryHandler : IRequestHandler<GetRevokedClientsForServerQuery, IEnumerable<ClientDto>>
    {
        private readonly ITunnelContext _context;

        public GetRevokedClientsForServerQueryHandler(ITunnelContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClientDto>> Handle(GetRevokedClientsForServerQuery query, CancellationToken cancellationToken)
        {
            return await _context.Clients
                .Where(x => x.ServerId == query.Id && x.IsRevoked)
                .Select(x => new ClientDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsRevoked = x.IsRevoked
                })
                .ToListAsync(cancellationToken);
        }
    }
}