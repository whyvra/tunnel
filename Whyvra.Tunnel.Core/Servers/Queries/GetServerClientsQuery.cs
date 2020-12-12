using System.Collections.Generic;
using MediatR;
using Whyvra.Tunnel.Common.Models;

namespace Whyvra.Tunnel.Core.Servers.Queries
{
    public class GetServerClientsQuery : IRequest<IEnumerable<ClientDto>>
    {
        public int Id { get; set; }
    }
}