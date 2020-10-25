using Microsoft.Extensions.Configuration;

namespace Whyvra.Tunnel.Data.Providers
{
    public class ConfigConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IConfiguration _config;

        public ConfigConnectionStringProvider(IConfiguration config)
        {
            _config = config;
        }

        public string ConnectionString => _config.GetConnectionString(nameof(TunnelContext));
    }
}