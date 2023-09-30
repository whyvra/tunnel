using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Whyvra.Tunnel.Data.Configuration;
using Whyvra.Tunnel.Data.Tracking;
using Whyvra.Tunnel.Domain;
using Whyvra.Tunnel.Domain.Entitites;

namespace Whyvra.Tunnel.Data
{
    public class TunnelContext : EventTrackingDbContext, ITunnelContext
    {
        public TunnelContext(DbContextOptions<TunnelContext> opts) : base(opts)
        {
        }

        public TunnelContext(DbContextOptions opts, IEventTracker tracker) : base(opts, tracker)
        {
        }

        public virtual DbSet<Client> Clients { get; set; }

        public virtual DbSet<ClientNetworkAddress> ClientNetworkAddresses { get; set; }

        public virtual DbSet<NetworkAddress> NetworkAddresses { get; set; }

        public virtual DbSet<Server> Servers { get ;set; }

        public virtual DbSet<ServerNetworkAddress> ServerNetworkAddresses { get; set; }

        public virtual DbSet<User> Users { get; set; }

        protected override int? GetUserId(Guid uid)
        {
            return Users.SingleOrDefault(x => x.Uid.Equals(uid))?.Id;
        }

        protected override async Task<int?> GetUserIdAsync(Guid uid, CancellationToken cancellationToken)
        {
            return (await Users.SingleOrDefaultAsync(x => x.Uid.Equals(uid), cancellationToken))?.Id;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            var method = typeof(TunnelFunctions)
                .GetRuntimeMethod(nameof(TunnelFunctions.Text), new [] { typeof((System.Net.IPAddress, int)) });

            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                modelBuilder.ApplyDataFixForSqlite();
                modelBuilder.HasDbFunction(method, builder => builder.HasName("LOWER").HasParameter("cidr").HasStoreType("string"));
            }
            else
            {
                modelBuilder
                    .HasDbFunction(method)
                    .HasTranslation(args => new SqlFunctionExpression(
                        "TEXT",
                        args,
                        nullable: true,
                        argumentsPropagateNullability: Enumerable.Repeat(true, args.Count),
                        typeof(string),
                        new StringTypeMapping("string", DbType.String)
                    ));
            }
        }
    }
}