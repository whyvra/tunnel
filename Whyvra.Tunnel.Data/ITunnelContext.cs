using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Whyvra.Tunnel.Domain.Entities;

namespace Whyvra.Tunnel.Data
{
    public interface ITunnelContext : IDisposable
    {
        DatabaseFacade Database { get; }

        DbSet<Client> Clients { get; set; }

        DbSet<ClientNetworkAddress> ClientNetworkAddresses { get; set; }

        DbSet<NetworkAddress> NetworkAddresses { get; set; }

        DbSet<Event> Events { get; set; }

        DbSet<Server> Servers { get ;set; }

        DbSet<ServerNetworkAddress> ServerNetworkAddresses { get; set; }

        DbSet<User> Users { get; set; }

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}