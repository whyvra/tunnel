using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Whyvra.Tunnel.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                    .UseSerilog((context, loggerConfiguration) =>
                        loggerConfiguration
                            .ReadFrom.Configuration(context.Configuration)
                            .Enrich.WithProperty("MachineName", Environment.MachineName)
                    )
                    .UseStartup<Startup>()
                );
    }
}
