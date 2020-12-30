using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Whyvra.Tunnel.Data;
using Whyvra.Tunnel.Data.Configuration;
using Whyvra.Tunnel.Domain.Entitites;

namespace Whyvra.Tunnel.Core.Clients.Commands
{
    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, int>
    {
        private readonly ClientValidator _clientValidator;
        private readonly ITunnelContext _context;

        public CreateClientCommandHandler(ClientValidator clientValidator, ITunnelContext context)
        {
            _clientValidator = clientValidator;
            _context = context;
        }

        public async Task<int> Handle(CreateClientCommand command, CancellationToken cancellationToken)
        {
            var server = await _context.Servers.AsNoTracking().SingleOrDefaultAsync(x => x.Id == command.ServerId, cancellationToken);
            if (server == null) throw new NullReferenceException($"A server with id #{command.ServerId} could not be found.");

            // Ensure client is unique
            await _clientValidator.EnsureUniqueClientOnCreate(
                command.Client.Name,
                command.Client.AssignedIp,
                command.ServerId,
                command.Client.IsIpAutoGenerated == true,
                cancellationToken
            );

            var network = IPNetwork.Parse($"{server.AssignedRange.addr}/{server.AssignedRange.cidr}");

            // Handle IP address
            if (command.Client.IsIpAutoGenerated == true)
            {
                // Get a list of all usable addresses
                var available = network.ListIPAddress(FilterEnum.Usable).ToList();
                available.Remove(network.FirstUsable);

                var addresses = await _context.Clients
                    .AsNoTracking()
                    .Where(x => x.ServerId == command.ServerId)
                    .Select(x => x.AssignedIp.addr)
                    .ToListAsync();

                // Remove the ones that are in use
                foreach (var addr in addresses)
                {
                    available.Remove(addr);
                }

                // Grab the first remaining one
                command.Client.AssignedIp = $"{available.First()}/32";
            } else {
                // Check that assigned IP is in range
                var isInRange = network.Contains(command.Client.AssignedIp.ToAddress().addr);
                if (!isInRange) throw new ArgumentException($"Assigned IP address was not in range {server.AssignedRange.addr}/{server.AssignedRange.cidr}");
            }

            var client = new WireguardClient
            {
                Name = command.Client.Name,
                Description = command.Client.Description,
                AssignedIp = command.Client.AssignedIp.ToAddress(),
                IsRevoked = command.Client.IsRevoked ?? false,
                PublicKey = command.Client.PublicKey,
                ServerId = command.ServerId
            };

            await _context.Clients.AddAsync(client, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return client.Id;
        }
    }
}