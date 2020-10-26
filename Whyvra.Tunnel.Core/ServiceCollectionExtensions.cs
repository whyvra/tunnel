using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Whyvra.Tunnel.Data;

namespace Whyvra.Tunnel.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTunnelHandlers(this IServiceCollection services)
        {
            return services
                .AddScoped<IEventTracker, EventTracker>()
                .AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
