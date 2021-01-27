using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Whyvra.Tunnel.Common.Configuration;

namespace Whyvra.Tunnel.Api.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwt(this IServiceCollection services, AuthOptions opts)
        {
            if (opts.Enabled)
            {
                services
                    .AddAuthentication(x =>
                    {
                        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(x =>
                    {
                        x.Audience = opts.ClientId;
                        x.Authority = opts.Authority;
                        x.MetadataAddress = $"{opts.Authority}/.well-known/openid-configuration";
                        x.RequireHttpsMetadata = true;

                        x.TokenValidationParameters.ClockSkew = TimeSpan.Zero;

                        x.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = context =>
                            {
                                context.NoResult();
                                context.Response.StatusCode = 401;
                                return Task.CompletedTask;
                            }
                        };
                    });

                // Add a required role, if specified in appsettings
                if (!string.IsNullOrWhiteSpace(opts.RequiredRole))
                {
                    services.AddAuthorization(x => x.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireRole(opts.RequiredRole)
                        .Build());
                }
            }

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, AuthOptions opts)
        {
            return services
                .AddSwaggerGen(x =>
                {
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                    x.IncludeXmlComments(xmlPath);

                    if (opts.Enabled)
                    {
                        // Enable JWT authentication
                        x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                        {
                            In = ParameterLocation.Header,
                            Description = "Please insert JWT with Bearer into field",
                            Name = "Authorization",
                            Type = SecuritySchemeType.ApiKey
                        });
                        x.AddSecurityRequirement(new OpenApiSecurityRequirement {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                Array.Empty<string>()
                            }
                        });
                    }
                });
        }
    }
}