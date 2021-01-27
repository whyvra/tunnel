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
    public class GetServerClientsQueryHandler : IRequestHandler<GetServerClientsQuery, IEnumerable<ClientDto>>
    {
        private readonly ITunnelContext _context;

        public GetServerClientsQueryHandler(ITunnelContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClientDto>> Handle(GetServerClientsQuery query, CancellationToken cancellationToken)
        {
            return await _context.Clients
                .AsNoTracking()
                .Where(x => !x.IsRevoked && x.ServerId == query.Id)
                .Select(x => new ClientDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    AssignedIp = TunnelFunctions.Text(x.AssignedIp),
                    PublicKey = x.PublicKey
                })
                .ToListAsync(cancellationToken);
        }
    }
}