using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Whyvra.Tunnel.Common.Models;
using Whyvra.Tunnel.Common.Queries;
using Whyvra.Tunnel.Data;

namespace Whyvra.Tunnel.Core.Clients.Queries
{
    public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, ClientDto>
    {
        private readonly ITunnelContext _context;

        public GetClientByIdQueryHandler(ITunnelContext context)
        {
            _context = context;
        }

        public async Task<ClientDto> Handle(GetClientByIdQuery query, CancellationToken cancellationToken)
        {
            var client = await _context.Clients
                .Select(x => new ClientDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Name,
                    AllowedIps = x.AllowedIps
                        .Select(n => $"{n.NetworkAddress.Address.addr}/{n.NetworkAddress.Address.cidr}")
                        .ToList(),
                    AssignedIp = $"{x.AssignedIp.addr}/{x.AssignedIp.cidr}",
                    IsRevoked = x.IsRevoked,
                    PublicKey = x.PublicKey,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .SingleOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

            if (client == null) throw new NullReferenceException($"Cannot find client with id #{query.Id}");

            return client;
        }
    }
}