using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Whyvra.Tunnel.Common.Models;
using Whyvra.Tunnel.Presentation.Configuration;

namespace Whyvra.Tunnel.Presentation.Services
{
    public class ClientService : INetworkEntityService
    {
        private readonly HttpClient _http;
        private readonly ApiOptions _api;

        public ClientService(IHttpClientFactory factory, IOptions<ApiOptions> api)
        {
            _http = factory.CreateClient("TunnelApi");
            _api = api.Value;
        }

        public async Task AddNetworkAddress(int id, int networkAddressId)
        {
            var result = await _http.PutAsync($"{_api.Url}/clients/{id}/allowedips/{networkAddressId}", null);
            result.EnsureSuccessStatusCode();
        }

        public async Task<int> CreateNew(int serverId, CreateUpdateClientDto client)
        {
            var result = await _http.PostAsJsonAsync($"{_api.Url}/servers/{serverId}/clients", client);
            result.EnsureSuccessStatusCode();

            var response = await result.Content.ReadFromJsonAsync<IdResponse>();
            return response.Id;
        }

        public async Task<ClientDto> Get(int id)
        {
            return await _http.GetFromJsonAsync<ClientDto>($"{_api.Url}/clients/{id}");
        }

        public async Task RemoveNetworkAddress(int id, int networkAddressId)
        {
            var result = await _http.DeleteAsync($"{_api.Url}/clients/{id}/allowedips/{networkAddressId}");
            result.EnsureSuccessStatusCode();
        }
    }
}