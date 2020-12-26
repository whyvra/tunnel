using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Whyvra.Tunnel.Common.Models;
using Whyvra.Tunnel.Presentation.Configuration;

namespace Whyvra.Tunnel.Presentation.Services
{
    public class UserService
    {
        private readonly HttpClient _http;
        private readonly ApiOptions _api;

        public UserService(IHttpClientFactory factory, IOptions<ApiOptions> api)
        {
            _http = factory.CreateClient("TunnelApi");
            _api = api.Value;
        }

        public async Task<int> CreateUpdateUser()
        {
            var result = await _http.PostAsync($"{_api.Url}/users", null);
            await result.EnsureSuccess();

            var response = await result.Content.ReadFromJsonAsync<IdResponse>();
            return response.Id;
        }
    }
}