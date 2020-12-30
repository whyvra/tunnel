using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Whyvra.Tunnel.Data;
using Whyvra.Tunnel.Data.Configuration;
using Whyvra.Tunnel.Domain.Entitites;

namespace Whyvra.Tunnel.Core.Servers.Commands
{
    public class CreateServerCommandHandler : IRequestHandler<CreateServerCommand, int>
    {
        private readonly ITunnelContext _context;

        public CreateServerCommandHandler(ITunnelContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateServerCommand command, CancellationToken cancellationToken)
        {
            var exists = await _context.Servers.AnyAsync(x => x.Name == command.Server.Name, cancellationToken);
            if (exists) throw new ArgumentException($"A server with name '{command.Server.Name}' already exists.");

            var server = new WireguardServer
            {
                Name = command.Server.Name,
                Description = command.Server.Description,
                AssignedRange = command.Server.AssignedRange.ToAddress(),
                Dns = IPAddress.Parse(command.Server.Dns),
                Endpoint = command.Server.Endpoint,
                ListenPort = command.Server.ListenPort,
                PublicKey = command.Server.PublicKey
            };

            await _context.Servers.AddAsync(server, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return server.Id;
        }
    }
}