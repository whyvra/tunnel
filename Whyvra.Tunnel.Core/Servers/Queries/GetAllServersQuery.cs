using System.Collections.Generic;
using MediatR;
using Whyvra.Tunnel.Common.Models;

namespace Whyvra.Tunnel.Core.Servers.Queries
{
    public class GetAllServersQuery : IRequest<IEnumerable<ServerDto>>
    {
    }
}