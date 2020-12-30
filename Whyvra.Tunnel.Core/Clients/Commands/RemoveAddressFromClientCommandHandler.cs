using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Whyvra.Tunnel.Data;

namespace Whyvra.Tunnel.Core.Clients.Commands
{
    public class RemoveAddressFromClientCommandHandler : IRequestHandler<RemoveAddressFromClientCommand>
    {
        private readonly ITunnelContext _context;

        public RemoveAddressFromClientCommandHandler(ITunnelContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemoveAddressFromClientCommand command, CancellationToken cancellationToken)
        {
            var clientAddress = await _context.ClientNetworkAddresses
                .SingleOrDefaultAsync(x => x.ClientId == command.ClientId && x.NetworkAddressId == command.NetworkAddressId);

            if (clientAddress == null)
            {
                throw new NullReferenceException($"A network address #{command.NetworkAddressId} on client #{command.ClientId} could not be found.");
            }

            _context.ClientNetworkAddresses.Remove(clientAddress);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}