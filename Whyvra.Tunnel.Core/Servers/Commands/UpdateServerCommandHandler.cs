using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Whyvra.Tunnel.Data;
using Whyvra.Tunnel.Data.Configuration;

namespace Whyvra.Tunnel.Core.Servers.Commands
{
    public class UpdateServerCommandHandler : IRequestHandler<UpdateServerCommand>
    {
        private readonly ITunnelContext _context;

        public UpdateServerCommandHandler(ITunnelContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateServerCommand command, CancellationToken cancellationToken)
        {
            // Get server with specified id
            var server = await _context.Servers
                .SingleOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

            // Throw an error if not found
            if (server == null) throw new NullReferenceException($"Cannot find server with id #{command.Id}");

            // Ensure name is unique
            var exists = await _context.Servers.AnyAsync(x => x.Name == command.Server.Name && x.Id != command.Id, cancellationToken);
            if (exists) throw new ArgumentException($"A server with name '{command.Server.Name}' already exists.");

            // Update server properties
            server.Name = command.Server.Name;
            server.Description = command.Server.Description;
            server.AssignedRange = command.Server.AssignedRange.ToAddress();
            server.Dns = IPAddress.Parse(command.Server.Dns);
            server.Endpoint = command.Server.Endpoint;
            server.ListenPort = command.Server.ListenPort;
            server.PublicKey = command.Server.PublicKey;

            // Save changes
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}