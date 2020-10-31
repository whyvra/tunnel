using System;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Whyvra.Tunnel.Presentation.Configuration;
using Whyvra.Tunnel.Presentation.Services;

namespace Whyvra.Tunnel.Presentation
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTunnel(this IServiceCollection services, WebAssemblyHostBuilder builder)
        {
            // Add options for the API
            builder.Services
                .AddOptions<ApiOptions>()
                .Configure(x => builder.Configuration.Bind("api", x));

            // Add options for authentication
            builder.Services
                .AddOptions<AuthenticationOptions>()
                .Configure(x => builder.Configuration.Bind("auth", x));

            // Get values from config
            var apiUrl = builder.Configuration.GetValue<string>("api:url");
            var auth = builder.Configuration.GetValue<bool>("auth:enabled");

            // Register HTTP client
            var apiClient = builder.Services.AddHttpClient("TunnelApi", x => x.BaseAddress = new Uri(apiUrl));
            if (auth)
            {
                // Add authorization handler if authentication is enabled
                apiClient.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
            }

            // Register services
            builder.Services
                .AddScoped<ServerService>();

            return services;
        }
    }
}