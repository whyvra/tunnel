using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Whyvra.Tunnel.Data;
using Whyvra.Tunnel.Data.Configuration;

namespace Whyvra.Tunnel.Core.Clients.Commands
{
    public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand>
    {
        private readonly ITunnelContext _context;

        public UpdateClientCommandHandler(ITunnelContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateClientCommand command, CancellationToken cancellationToken)
        {
            var client = await _context.Clients
                .SingleOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

            if (client == null) throw new NullReferenceException($"Client with id #{command.Id} could not be found.");

            client.Name = command.Client.Name;
            client.Description = command.Client.Description;
            client.AssignedIp = command.Client.AssignedIp.ToAddress();
            client.IsRevoked = command.Client.IsRevoked ?? client.IsRevoked;
            client.PublicKey = command.Client.PublicKey;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}