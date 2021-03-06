using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Whyvra.Tunnel.Common.Models;
using Whyvra.Tunnel.Presentation.Configuration;

namespace Whyvra.Tunnel.Presentation.Services
{
    public class ServerService : INetworkEntityService
    {
        private readonly HttpClient _http;
        private readonly ApiOptions _api;

        public ServerService(IHttpClientFactory factory, IOptions<ApiOptions> api)
        {
            _http = factory.CreateClient("TunnelApi");
            _api = api.Value;
        }

        public async Task AddNetworkAddress(int id, int networkAddressId)
        {
            var result = await _http.PutAsync($"{_api.Url}/servers/{id}/allowedrange/{networkAddressId}", null);
            await result.EnsureSuccess();
        }

        public async Task<int> CreateNew(CreateUpdateServerDto server)
        {
            var result = await _http.PostAsJsonAsync($"{_api.Url}/servers", server);
            await result.EnsureSuccess();

            var response = await result.Content.ReadFromJsonAsync<IdResponse>();
            return response.Id;
        }

        public async Task Delete(int id)
        {
            var result = await _http.DeleteAsync($"{_api.Url}/servers/{id}");
            await result.EnsureSuccess();
        }

        public async Task<ServerDto> Get(int id)
        {
            return await _http.GetFromJsonAsyncWithSuccess<ServerDto>($"{_api.Url}/servers/{id}");
        }

        public async Task<IEnumerable<ServerDto>> GetAll()
        {
            return await _http.GetFromJsonAsyncWithSuccess<IEnumerable<ServerDto>>($"{_api.Url}/servers");
        }

        public async Task<IEnumerable<ClientDto>> GetClients(int id)
        {
            return await _http.GetFromJsonAsyncWithSuccess<IEnumerable<ClientDto>>($"{_api.Url}/servers/{id}/clients");
        }

        public async Task<IEnumerable<ClientDto>> GetRevokedClients(int id)
        {
            return await _http.GetFromJsonAsyncWithSuccess<IEnumerable<ClientDto>>($"{_api.Url}/servers/{id}/clients/revoked");
        }

        public async Task RemoveNetworkAddress(int id, int networkAddressId)
        {
            var result = await _http.DeleteAsync($"{_api.Url}/servers/{id}/allowedrange/{networkAddressId}");
            await result.EnsureSuccess();
        }
        public async Task Update(int id, CreateUpdateServerDto server)
        {
            var result = await _http.PutAsJsonAsync($"{_api.Url}/servers/{id}", server);
            await result.EnsureSuccess();
        }
    }
}
