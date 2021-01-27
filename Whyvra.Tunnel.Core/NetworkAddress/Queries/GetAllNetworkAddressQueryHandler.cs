using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Whyvra.Tunnel.Common.Models;
using Whyvra.Tunnel.Data;

namespace Whyvra.Tunnel.Core.NetworkAddress.Queries
{
    public class GetAllNetworkAddressQueryHandler : IRequestHandler<GetAllNetworkAddressQuery, IEnumerable<NetworkAddressDto>>
    {
        private readonly ITunnelContext _context;

        public GetAllNetworkAddressQueryHandler(ITunnelContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NetworkAddressDto>> Handle(GetAllNetworkAddressQuery query, CancellationToken cancellationToken)
        {
            return await _context.NetworkAddresses
                .AsNoTracking()
                .Select(x => new NetworkAddressDto
                {
                    Id = x.Id,
                    Address = TunnelFunctions.Text(x.Address)
                })
                .ToListAsync(cancellationToken);
        }
    }
}