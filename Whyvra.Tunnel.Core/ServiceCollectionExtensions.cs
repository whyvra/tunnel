using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Whyvra.Tunnel.Core.Clients;
using Whyvra.Tunnel.Data;

namespace Whyvra.Tunnel.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTunnelHandlers(this IServiceCollection services)
        {
            return services
                .AddScoped<IEventTracker, EventTracker>()
                .AddScoped<ClientValidator>()
                .AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
