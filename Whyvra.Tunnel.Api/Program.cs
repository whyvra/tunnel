using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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
                .ConfigureAppConfiguration((context, config) => {
                    var custom = context.Configuration.GetValue<string>("CUSTOM_APPSETTINGS");
                    if (!string.IsNullOrWhiteSpace(custom))
                    {
                        Console.WriteLine($"Loading custom configuration : {custom}");
                        config.AddJsonFile(custom);
                    }
                    config.AddEnvironmentVariables();
                })
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
