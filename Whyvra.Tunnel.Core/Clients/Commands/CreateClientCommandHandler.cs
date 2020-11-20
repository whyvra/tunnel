using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Whyvra.Tunnel.Data;
using Whyvra.Tunnel.Data.Configuration;
using Whyvra.Tunnel.Domain.Entitites;

namespace Whyvra.Tunnel.Core.Clients.Commands
{
    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, int>
    {
        private readonly ITunnelContext _context;

        public CreateClientCommandHandler(ITunnelContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateClientCommand command, CancellationToken cancellationToken)
        {
            var exists = await _context.Servers.AnyAsync(x => x.Id == command.ServerId, cancellationToken);
            if (!exists) throw new NullReferenceException($"Cannot find server with id #{command.ServerId}");

            var client = new WireguardClient
            {
                Name = command.Client.Name,
                Description = command.Client.Description,
                AssignedIp = command.Client.AssignedIp.ToAddress(),
                IsRevoked = command.Client.IsRevoked ?? false,
                PublicKey = command.Client.PublicKey,
                ServerId = command.ServerId
            };

            await _context.Clients.AddAsync(client, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return client.Id;
        }
    }
}