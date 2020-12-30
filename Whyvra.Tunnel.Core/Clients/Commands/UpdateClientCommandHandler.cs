using System;
using System.Linq;
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
        private readonly ClientValidator _clientValidator;
        private readonly ITunnelContext _context;

        public UpdateClientCommandHandler(ClientValidator clientValidator, ITunnelContext context)
        {
            _clientValidator = clientValidator;
            _context = context;
        }

        public async Task<Unit> Handle(UpdateClientCommand command, CancellationToken cancellationToken)
        {
            var client = await _context.Clients
                .SingleOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

            if (client == null) throw new NullReferenceException($"Client with id #{command.Id} could not be found.");

            // Ensure client is unique
            await _clientValidator.EnsureUniqueClientOnUpdate(
                client.Id,
                command.Client.Name,
                command.Client.AssignedIp,
                client.ServerId,
                cancellationToken
            );

            // Update properties
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