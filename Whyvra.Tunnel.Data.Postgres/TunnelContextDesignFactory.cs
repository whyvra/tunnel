using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Whyvra.Tunnel.Data.Postgres
{
    public class TunnelContextDesignFactory : IDesignTimeDbContextFactory<TunnelContext>
    {
        public TunnelContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            var connectionString = configuration.GetConnectionString(nameof(TunnelContext));
            var builder = new DbContextOptionsBuilder<TunnelContext>();
            builder.UseNpgsql(connectionString, x => x.MigrationsAssembly(assemblyName));

            return new TunnelContext(builder.Options);
        }
    }
}