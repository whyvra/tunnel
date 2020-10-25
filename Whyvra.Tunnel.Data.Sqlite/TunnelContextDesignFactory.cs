using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Whyvra.Tunnel.Data.Providers;

namespace Whyvra.Tunnel.Data.Sqlite
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
            var provider = new ConfigConnectionStringProvider(configuration);
            var builder = new DbContextOptionsBuilder<TunnelContext>();
            builder.UseSqlite(provider.ConnectionString, x => x.MigrationsAssembly(assemblyName));

            return new TunnelContext(builder.Options);
        }
    }
}
