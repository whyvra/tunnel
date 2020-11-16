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

            // Update server properties
            server.Name = command.Data.Name;
            server.Description = command.Data.Description;
            server.AssignedRange = command.Data.AssignedRange.ToAddress();
            server.Dns = IPAddress.Parse(command.Data.Dns);
            server.Endpoint = command.Data.Endpoint;
            server.PublicKey = command.Data.PublicKey;

            // Save changes
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}