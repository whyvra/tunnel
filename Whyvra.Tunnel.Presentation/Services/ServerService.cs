using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Whyvra.Tunnel.Common.Models;
using Whyvra.Tunnel.Presentation.Configuration;

namespace Whyvra.Tunnel.Presentation.Services
{
    public class ServerService
    {
        private readonly HttpClient _http;
        private readonly ApiOptions _api;

        public ServerService(IHttpClientFactory factory, IOptions<ApiOptions> api)
        {
            _http = factory.CreateClient("TunnelApi");
            _api = api.Value;
        }

        public async Task<IEnumerable<ServerDto>> GetAll()
        {
            return await _http.GetFromJsonAsync<IEnumerable<ServerDto>>($"{_api.Url}/servers");
        }
    }
}
