using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Whyvra.Tunnel.Common.Commands;
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
            var server = new WireguardServer
            {
                Name = command.Name,
                Description = command.Description,
                AssignedRange = command.AssignedRange.ToAddress(),
                Dns = IPAddress.Parse(command.Dns),
                Endpoint = command.Endpoint,
                PublicKey = command.PublicKey
            };

            await _context.Servers.AddAsync(server, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return server.Id;
        }
    }
}