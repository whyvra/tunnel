using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
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
                Name = command.Data.Name,
                Description = command.Data.Description,
                AssignedRange = command.Data.AssignedRange.ToAddress(),
                Dns = IPAddress.Parse(command.Data.Dns),
                Endpoint = command.Data.Endpoint,
                ListenPort = command.Data.ListenPort,
                PublicKey = command.Data.PublicKey
            };

            await _context.Servers.AddAsync(server, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return server.Id;
        }
    }
}