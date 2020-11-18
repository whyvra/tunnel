using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Whyvra.Tunnel.Data;

namespace Whyvra.Tunnel.Core.Servers.Commands
{
    public class DeleteServerCommandHandler : IRequestHandler<DeleteServerCommand>
    {
        private readonly ITunnelContext _context;

        public DeleteServerCommandHandler(ITunnelContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteServerCommand command, CancellationToken cancellationToken)
        {
            var server = await _context.Servers
                .SingleOrDefaultAsync(x => x.Id == command.Id);

            if (server == null ) throw new NullReferenceException($"Server with id #{command.Id} could not be found.");

            _context.Servers.Remove(server);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}