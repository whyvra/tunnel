using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Whyvra.Tunnel.Common.Models;
using Whyvra.Tunnel.Data;

namespace Whyvra.Tunnel.Core.Servers.Queries
{
    public class GetServerByIdQueryHandler : IRequestHandler<GetServerByIdQuery, ServerDto>
    {
        private readonly ITunnelContext _context;

        public GetServerByIdQueryHandler(ITunnelContext context)
        {
            _context = context;
        }

        public async Task<ServerDto> Handle(GetServerByIdQuery query, CancellationToken cancellationToken)
        {
            var server = await _context.Servers
                .AsNoTracking()
                .Select(x => new ServerDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    AssignedRange = $"{x.AssignedRange.addr}/{x.AssignedRange.cidr}",
                    Clients = x.Clients
                        .Select(c => new ClientDto
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Description = c.Description
                        })
                        .ToList(),
                    DefaultAllowedRange = x.DefaultAllowedRange
                        .Select(n => $"{n.NetworkAddress.Address.addr}/{n.NetworkAddress.Address.cidr}")
                        .ToList(),
                    Dns = x.Dns.ToString(),
                    Endpoint = x.Endpoint,
                    PublicKey = x.PublicKey,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .SingleOrDefaultAsync(x => x.Id == query.Id);

            if (server == null) throw new NullReferenceException($"Cannot find server with id #{query.Id}");

            return server;
        }
    }
}