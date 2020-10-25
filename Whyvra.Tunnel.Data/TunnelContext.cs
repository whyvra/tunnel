using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Whyvra.Tunnel.Data.Configuration;
using Whyvra.Tunnel.Domain;
using Whyvra.Tunnel.Domain.Entitites;

namespace Whyvra.Tunnel.Data
{
    public class TunnelContext : DbContext, ITunnelContext
    {
        private readonly IEventTracker _tracker;

        public TunnelContext(DbContextOptions<TunnelContext> opts) : base(opts)
        {
        }

        public TunnelContext(DbContextOptions<TunnelContext> opts, IEventTracker tracker) : base(opts)
        {
            _tracker = tracker;
        }

        public virtual DbSet<WireguardClient> Clients { get; set; }

        public virtual DbSet<ClientNetworkAddress> ClientNetworkAddresses { get; set; }

        public virtual DbSet<Event> Events { get; set; }

        public virtual DbSet<NetworkAddress> NetworkAddresses { get; set; }

        public virtual DbSet<WireguardServer> Servers { get ;set; }

        public virtual DbSet<ServerNetworkAddress> ServerNetworkAddresses { get; set; }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                modelBuilder.ApplyDataFixForSqlite();
            }
        }

        public override int SaveChanges()
        {
            // Current user lookup function
            int? GetUserId(Guid uid) => Users.SingleOrDefault(x => x.Uid.Equals(uid))?.Id;

            // Generate events based on tracked changes
            var entries = ChangeTracker.Entries<IEntity>();
            var events = _tracker.GetEvents(entries, GetUserId)
                .ToList();

            // Add eligible events
            _tracker.AddEvents(events, this);

            UpdateDateFields();

            // Save changes
            var count = base.SaveChanges();

            // Add remaining events and save again if needed
            var shouldSave = _tracker.AddRemainingEvents(events, this);
            if (shouldSave) base.SaveChanges();

            return count;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            // Current user lookup function
            int? GetUserId(Guid uid) => Users.SingleOrDefault(x => x.Uid.Equals(uid))?.Id;

            // Generate events based on tracked changes
            var entries = ChangeTracker.Entries<IEntity>();
            var events = _tracker.GetEvents(entries, GetUserId)
                .ToList();

            // Add eligible events
            await _tracker.AddEventsAsync(events, this, cancellationToken);

            UpdateDateFields();

            // Save changes
            var count = await base.SaveChangesAsync(cancellationToken);

            // Add remaining events and save again if needed
            var shouldSave = await _tracker.AddRemainingEventsAsync(events, this, cancellationToken);
            if (shouldSave) await base.SaveChangesAsync(cancellationToken);

            return count;
        }

        private void UpdateDateFields()
        {
            var entries = ChangeTracker.Entries();
            var now = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.Entity is IEntity entity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entity.UpdatedAt = now;
                            break;

                        case EntityState.Added:
                            entity.CreatedAt = now;
                            entity.UpdatedAt = now;
                            break;
                    }
                }

                if (entry.Entity is Event ev)
                {
                    ev.Timestamp = now;
                }
            }
        }
    }
}