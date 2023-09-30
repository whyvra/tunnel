using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Whyvra.Tunnel.Domain;
using Whyvra.Tunnel.Domain.Entities;

namespace Whyvra.Tunnel.Data.Tracking
{
    public abstract class EventTrackingDbContext : DbContext
    {
        private readonly IEventTracker _tracker;

        protected EventTrackingDbContext(DbContextOptions opts) : base(opts)
        {
        }

        protected EventTrackingDbContext(DbContextOptions opts, IEventTracker tracker) : base(opts)
        {
            _tracker = tracker;
        }

        public virtual DbSet<Event> Events { get; set; }

        protected abstract int? GetUserId(Guid uid);

        protected abstract Task<int?> GetUserIdAsync(Guid uid, CancellationToken cancellationToken);

        public override int SaveChanges()
        {
            // Generate events based on tracked changes
            var entries = ChangeTracker.Entries<IEntity>();
            var events = _tracker.GetEvents(entries, GetUserId).ToList();

            // Add eligible events
            _tracker.AddEvents(events, this);
            SetTrackableFields();

            // Save changes
            var count = base.SaveChanges();

            // Add remaining events and save again if needed
            var shouldSave = _tracker.AddRemainingEvents(events, this);
            if (shouldSave) base.SaveChanges();

            return count;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Generate events based on tracked changes
            var entries = ChangeTracker.Entries<IEntity>();
            var events = (await _tracker.GetEventsAsync(entries, GetUserIdAsync, cancellationToken)).ToList();

            // Add eligible events
            await _tracker.AddEventsAsync(events, this, cancellationToken);
            SetTrackableFields();

            // Save changes
            var count = await base.SaveChangesAsync(cancellationToken);

            // Add remaining events and save again if needed
            var shouldSave = await _tracker.AddRemainingEventsAsync(events, this, cancellationToken);
            if (shouldSave) await base.SaveChangesAsync(cancellationToken);

            return count;
        }
        private void SetTrackableFields()
        {
            var entries = ChangeTracker.Entries();
            var now = DateTime.UtcNow;

            foreach (var entry in entries.Where(x => x.Entity is IEntity))
            {
                var entity = entry.Entity as IEntity;
                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreatedAt = now;
                        entity.UpdatedAt = now;
                        break;
                    case EntityState.Modified:
                        entity.UpdatedAt = now;
                        break;
                }
            }

            foreach(var entry in entries.Where(x => x.Entity is Event))
            {
                var entity = entry.Entity as Event;
                entity.Timestamp = now;
            }
        }
    }
}
