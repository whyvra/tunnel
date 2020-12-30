using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Whyvra.Tunnel.Data;

namespace Whyvra.Tunnel.Core.Servers.Commands
{
    public class RemoveAddressFromServerCommandHandler : IRequestHandler<RemoveAddressFromServerCommand>
    {
        private readonly ITunnelContext _context;

        public RemoveAddressFromServerCommandHandler(ITunnelContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemoveAddressFromServerCommand command, CancellationToken cancellationToken)
        {
            var serverAddress = await _context.ServerNetworkAddresses
                .SingleOrDefaultAsync(x => x.NetworkAddressId == command.NetworkAddressId && x.ServerId == command.ServerId, cancellationToken);

            if (serverAddress == null)
            {
                throw new NullReferenceException($"A network address #{command.NetworkAddressId} on server #{command.ServerId} could not be found.");
            }

            _context.ServerNetworkAddresses.Remove(serverAddress);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}