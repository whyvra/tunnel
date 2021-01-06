using System.Reflection;
using System.Text.Json;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Whyvra.Tunnel.Api.Authentication;
using Whyvra.Tunnel.Api.Configuration;
using Whyvra.Tunnel.Api.Filters;
using Whyvra.Tunnel.Common.Configuration;
using Whyvra.Tunnel.Common.Models.Validation;
using Whyvra.Tunnel.Core;
using Whyvra.Tunnel.Core.Users;
using Whyvra.Tunnel.Data;
using Whyvra.Tunnel.Data.Providers;

namespace Whyvra.Tunnel.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Load auth options for configuration
            var authOptions = new AuthOptions();
            Configuration.Bind("auth", authOptions);

            // Register auth options
            services
                .AddOptions<AuthOptions>()
                .Configure(x => Configuration.Bind("auth", x));

            // Register JWT authentication if enabled
            services.AddJwt(authOptions);

            // Register controllers
            services.
                AddControllers(x =>
                {
                    x.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
                    x.Filters.Add(new HttpExceptionFilter());
                    x.Filters.Add(new ValidationFilter());
                    if (authOptions.Enabled)
                    {
                        x.Filters.Add(new AuthorizeFilter());
                    }
                })
                .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<CreateUpdateServerDtoValidator>())
                .AddJsonOptions(x =>
                {
                    x.JsonSerializerOptions.IgnoreNullValues = true;
                    x.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                })
                .ConfigureApplicationPartManager(x => x.FeatureProviders.Add(new AuthenticationFeatureProvider(authOptions.Enabled)));

            services.Configure<ForwardedHeadersOptions>(x =>
            {
                x.ForwardLimit = 2;
                x.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            // Register handlers
            services.AddTunnelHandlers();

            // Register DbContext
            services.AddScoped<IConnectionStringProvider, ConfigConnectionStringProvider>();
            services.AddDbContext<ITunnelContext, TunnelContext>((svc, opts) =>
            {
                var provider = svc.GetService<IConnectionStringProvider>();
                var database = Configuration.GetValue<string>("database:type");

                switch (database)
                {
                    case "postgres":
                        opts.UseNpgsql(provider.ConnectionString, x => x.MigrationsAssembly("Whyvra.Tunnel.Data.Postgres"));
                        break;
                    case "sqlite":
                        opts.UseSqlite(provider.ConnectionString, x => x.MigrationsAssembly("Whyvra.Tunnel.Data.Sqlite"));
                        break;
                }
            });

            // Register user manager
            services.AddHttpContextAccessor();
            services.AddScoped<IUserManager, HttpUserManager>();

            // Register swagger
            services.AddSwagger(authOptions);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var autoMigrationsEnabled = Configuration.GetValue<bool>("database:automaticMigrations");
            if (autoMigrationsEnabled)
            {
                using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
                using var context = scope.ServiceProvider.GetService<ITunnelContext>();

                context.Database.Migrate();
            }

            app.UseForwardedHeaders();

            app.UseSwagger();
            app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", Assembly.GetExecutingAssembly().GetName().Name));

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
