using System;
using FluentValidation;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Whyvra.Tunnel.Common.Models;
using Whyvra.Tunnel.Common.Models.Validation;
using Whyvra.Tunnel.Presentation.Authentication;
using Whyvra.Tunnel.Presentation.Configuration;
using Whyvra.Tunnel.Presentation.Services;
using Whyvra.Tunnel.Presentation.ViewModels;

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
                // Register custom provider and handle to use id_token instead of access_token for OIDC
                builder.Services
                    .AddScoped<IdTokenProvider>()
                    .AddScoped<CustomAuthorizationMessageHandler>()
                    .AddOidcAuthentication(x => builder.Configuration.Bind("auth", x.ProviderOptions));

                // Add authorization handler if authentication is enabled
                apiClient.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
            }

            // Register services
            builder.Services
                .AddScoped<NetworkAddressService>()
                .AddScoped<ServerService>()
                .AddScoped<UserService>();

            // Register validators
            builder.Services
                .AddScoped<IValidator<CreateUpdateServerDto>, CreateUpdateServerDtoValidator>()
                .AddScoped<IValidator<ServerViewModel>, ServerViewModelValidator>();

            return services;
        }
    }
}