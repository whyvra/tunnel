using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Whyvra.Tunnel.Domain;
using Whyvra.Tunnel.Domain.Entitites;

namespace Whyvra.Tunnel.Data
{
    public interface IEventTracker
    {
        void AddEvents(IEnumerable<(EntityEntry<IEntity> Entry, Event Event)> events, ITunnelContext context);

        Task AddEventsAsync(IEnumerable<(EntityEntry<IEntity> Entry, Event Event)> events, ITunnelContext context, CancellationToken cancellationToken);

        bool AddRemainingEvents(IEnumerable<(EntityEntry<IEntity> Entry, Event Event)> events, ITunnelContext context);

        Task<bool> AddRemainingEventsAsync(IEnumerable<(EntityEntry<IEntity> Entry, Event Event)> events, ITunnelContext context, CancellationToken cancellationToken);

        IEnumerable<(EntityEntry<IEntity> Entry, Event Event)> GetEvents(IEnumerable<EntityEntry<IEntity>> entries, Func<Guid, int?> getUserId);
    }
}