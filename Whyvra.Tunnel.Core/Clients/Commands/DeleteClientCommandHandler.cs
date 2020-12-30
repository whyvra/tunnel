using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Whyvra.Tunnel.Data;

namespace Whyvra.Tunnel.Core.Clients.Commands
{
    public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand>
    {
        private readonly ITunnelContext _context;

        public DeleteClientCommandHandler(ITunnelContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteClientCommand command, CancellationToken cancellationToken)
        {
            var client = await _context.Clients
                .SingleOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

            if (client == null) throw new NullReferenceException($"A client with id #{command.Id} could not be found.");

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}