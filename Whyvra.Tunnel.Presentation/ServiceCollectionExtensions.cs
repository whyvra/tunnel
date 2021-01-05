using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Bunit;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QRCoder;
using Whyvra.Tunnel.Common.Configuration;
using Whyvra.Tunnel.Common.Models;
using Whyvra.Tunnel.Common.Models.Validation;
using Whyvra.Tunnel.Presentation.Authentication;
using Whyvra.Tunnel.Presentation.Configuration;
using Whyvra.Tunnel.Presentation.Logging;
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
                .AddOptions<AuthOptions>()
                .Configure(x => builder.Configuration.Bind("auth", x));

            // Get values from config
            var apiUrl = builder.Configuration.GetValue<string>("api:url");
            var auth = builder.Configuration.GetValue<bool>("auth:enabled");

            // Register HTTP client
            var apiClient = builder.Services.AddHttpClient("TunnelApi", x => {
                var url = apiUrl.StartsWith("/")
                    ? builder.HostEnvironment.BaseAddress + apiUrl
                    : apiUrl;

                x.BaseAddress = new Uri(url);
            });
            if (auth)
            {
                // Register custom provider and handle to use id_token instead of access_token for OIDC
                builder.Services
                    .AddScoped<IdTokenProvider>()
                    .AddScoped<CustomAuthorizationMessageHandler>()
                    .AddOidcAuthentication(x => {
                        builder.Configuration.Bind("auth", x.ProviderOptions);
                        if (string.IsNullOrWhiteSpace(x.ProviderOptions.ResponseType))
                        {
                            x.ProviderOptions.ResponseType = "code";
                        }
                    });

                // Add authorization handler if authentication is enabled
                apiClient.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

                // Check if role is specified
                var role = builder.Configuration.GetValue<string>("auth:requiredRole");
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser();

                if (!string.IsNullOrWhiteSpace(role))
                {
                    // Require user to have claim with role
                    policy.RequireAssertion(x => {
                        var claim = x.User.Claims.SingleOrDefault(x => x.Type.Equals("roles"));
                        if (claim != null) {
                            var roles = JsonSerializer.Deserialize<IEnumerable<string>>(claim.Value);
                            return roles.Contains(role);
                        }

                        return false;
                    });
                }

                // Add policy which requires authenticated users and optionally a custom role claim
                builder.Services.AddAuthorizationCore(x => x.AddPolicy("WireGuard", policy.Build()));
            }

            // Register service dependencies
            builder.Services
                .AddScoped<QRCodeGenerator>()
                .AddScoped<TestContext>();

            // Register services
            builder.Services
                .AddScoped<ClientService>()
                .AddScoped<NetworkAddressService>()
                .AddScoped<QrCodeService>()
                .AddScoped<ServerService>()
                .AddScoped<TemplateService>()
                .AddScoped<UserService>();

            // Register validators
            builder.Services
                .AddScoped<IValidator<CreateClientDto>>(_ => new CreateClientDtoValidator(false))
                .AddScoped<IValidator<CreateUpdateServerDto>, CreateUpdateServerDtoValidator>()
                .AddScoped<IValidator<UpdateClientDto>, UpdateClientDtoValidator>()
                .AddScoped<IValidator<ClientViewModel>, ClientViewModelValidator<ClientViewModel, CreateClientDto>>()
                .AddScoped<IValidator<ServerViewModel>, ServerViewModelValidator>()
                .AddScoped<IValidator<UpdateClientViewModel>, ClientViewModelValidator<UpdateClientViewModel, UpdateClientDto>>();

            // Setup logger to catch unhandled exception
            var handler = new ExceptionHandler();
            var provider = new ExceptionLoggerProvider(handler);
            builder.Services.AddSingleton<IExceptionHandler>(handler);
            builder.Logging.AddProvider(provider);

            return services;
        }
    }
}