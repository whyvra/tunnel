using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Whyvra.Tunnel.Common.Commands;
using Whyvra.Tunnel.Data;
using Whyvra.Tunnel.Data.Configuration;

namespace Whyvra.Tunnel.Core.NetworkAddress.Commands
{
    public class CreateNetworkAddressCommandHandler : IRequestHandler<CreateNetworkAddressCommand, int>
    {
        private readonly ITunnelContext _context;

        public CreateNetworkAddressCommandHandler(ITunnelContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateNetworkAddressCommand command, CancellationToken cancellationToken)
        {
            var address = new Domain.Entitites.NetworkAddress
            {
                Address = command.Address.ToAddress()
            };

            await _context.NetworkAddresses.AddAsync(address, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return address.Id;
        }
    }
}