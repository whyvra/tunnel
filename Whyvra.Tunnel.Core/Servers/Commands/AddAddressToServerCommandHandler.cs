using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Whyvra.Tunnel.Data;
using Whyvra.Tunnel.Domain.Entitites;

namespace Whyvra.Tunnel.Core.Servers.Commands
{
    public class AddAddressToServerCommandHandler : IRequestHandler<AddAddressToServerCommand, int>
    {
        private readonly ITunnelContext _context;

        public AddAddressToServerCommandHandler(ITunnelContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddAddressToServerCommand command, CancellationToken cancellationToken)
        {
            var serverAddress = new ServerNetworkAddress
            {
                NetworkAddressId = command.NetworkAddressId,
                ServerId = command.ServerId
            };

            await _context.ServerNetworkAddresses.AddAsync(serverAddress, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return serverAddress.Id;
        }
    }
}