using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Whyvra.Tunnel.Data;
using Whyvra.Tunnel.Domain.Entitites;

namespace Whyvra.Tunnel.Core.Clients.Commands
{
    public class AddAddressToClientCommandHandler : IRequestHandler<AddAddressToClientCommand, int>
    {
        private readonly ITunnelContext _context;

        public AddAddressToClientCommandHandler(ITunnelContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddAddressToClientCommand command, CancellationToken cancellationToken)
        {
            var clientAddress = new ClientNetworkAddress
            {
                ClientId = command.ClientId,
                NetworkAddressId = command.NetworkAddressId,
            };

            await _context.ClientNetworkAddresses.AddAsync(clientAddress, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return clientAddress.Id;
        }
    }
}