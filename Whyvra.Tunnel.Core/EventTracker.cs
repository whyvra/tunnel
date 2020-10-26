using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Whyvra.Tunnel.Common.Json;
using Whyvra.Tunnel.Core.Users;
using Whyvra.Tunnel.Data;
using Whyvra.Tunnel.Domain;
using Whyvra.Tunnel.Domain.Entitites;

namespace Whyvra.Tunnel.Core
{
    public class EventTracker : IEventTracker
    {
        private readonly IUserManager _userManager;

        public EventTracker(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public void AddEvents(IEnumerable<(EntityEntry<IEntity> Entry, Event Event)> events, ITunnelContext context)
        {
            var toAdd = events
                .Where(x => x.Event.RecordId != 0)
                .Select(x => x.Event)
                .ToList();

            context.Events.AddRange(toAdd);
        }

        public async Task AddEventsAsync(IEnumerable<(EntityEntry<IEntity> Entry, Event Event)> events, ITunnelContext context, CancellationToken cancellationToken)
        {
            var toAdd = events
                .Where(x => x.Event.RecordId != 0)
                .Select(x => x.Event)
                .ToList();

            await context.Events.AddRangeAsync(toAdd, cancellationToken);
        }

        public bool AddRemainingEvents(IEnumerable<(EntityEntry<IEntity> Entry, Event Event)> events, ITunnelContext context)
        {
            var toAdd = ProcessRemaining(events);
            if (!toAdd.Any()) return false;

            context.Events.AddRange(toAdd.Select(x => x.Event));

            return true;
        }

        public async Task<bool> AddRemainingEventsAsync(IEnumerable<(EntityEntry<IEntity>, Event)> events, ITunnelContext context, CancellationToken cancellationToken)
        {
            var toAdd = ProcessRemaining(events);
            if (!toAdd.Any()) return false;

            await context.Events.AddRangeAsync(toAdd.Select(x => x.Event), cancellationToken);

            return true;
        }

        public IEnumerable<(EntityEntry<IEntity> Entry, Event Event)> GetEvents(IEnumerable<EntityEntry<IEntity>> entries, Func<Guid, int?> getUserId)
        {
            var sourceAddress = _userManager.GetSourceAddress();
            var userId = getUserId(_userManager.GetUserUid());

            var events = entries
                .Where(x => x.State != EntityState.Unchanged && x.State != EntityState.Detached)
                .Select(x => {
                    var ev = new Event
                    {
                        EventType = Enum.GetName(typeof(EntityState), x.State),
                        Data = SerializeData(x),
                        SourceAddress = sourceAddress,
                        TableId = x.Metadata.GetTableName()
                    };

                    if (x.State != EntityState.Added) ev.RecordId = x.Entity.Id;
                    if (!(x.Entity is User) && userId.HasValue) ev.UserId = userId.Value;

                    return (x, ev);
                });

            return events;
        }

        private static List<(EntityEntry<IEntity> Entry, Event Event)> ProcessRemaining(IEnumerable<(EntityEntry<IEntity>, Event)> events)
        {
            var toAdd = events
                .Where(x => x.Item2.RecordId == 0)
                .ToList();

            toAdd.ForEach(x =>
            {
                var (entry, ev) = x;

                ev.RecordId = entry.Entity.Id;
                ev.Timestamp = entry.Entity.CreatedAt;

                if (entry.Entity is User) ev.UserId = entry.Entity.Id;
            });

            return toAdd;
        }

        private static JsonDocument SerializeData(EntityEntry entry)
        {
            if (entry.State != EntityState.Modified && entry.State != EntityState.Added) return null;

            var query = entry.Properties
                .Where(x => !x.Metadata.Name.Equals("Id") && !x.Metadata.Name.Equals("CreatedAt") && !x.Metadata.Name.Equals("UpdatedAt"));

            if (entry.State == EntityState.Modified)
            {
                query = query.Where(x => x.IsModified);
            }

            var data = query
                .Where(x => x.CurrentValue != null)
                .ToDictionary(x => x.Metadata.Name, x => x.CurrentValue);

            var opts = new JsonSerializerOptions { IgnoreNullValues = true };
            opts.Converters.Add(new IpAddressJsonConverter());
            opts.Converters.Add(new CidrAddressJsonConverter());

            var json = JsonSerializer.Serialize(data, opts);

            return JsonDocument.Parse(json);
        }
    }
}
