using System.Collections.Generic;
using MediatR;
using Whyvra.Tunnel.Common.Models;

namespace Whyvra.Tunnel.Core.NetworkAddress.Queries
{
    public class GetAllNetworkAddressQuery : IRequest<IEnumerable<NetworkAddressDto>>
    {
    }
}