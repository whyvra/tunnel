using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Whyvra.Tunnel.Common.Models;
using Whyvra.Tunnel.Data;

namespace Whyvra.Tunnel.Core.Servers.Queries
{
    public class GetAllServersQueryHandler : IRequestHandler<GetAllServersQuery, IEnumerable<ServerDto>>
    {
        private readonly ITunnelContext _context;

        public GetAllServersQueryHandler(ITunnelContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ServerDto>> Handle(GetAllServersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Servers
                .AsNoTracking()
                .Select(x => new ServerDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .ToListAsync(cancellationToken);
        }
    }
}