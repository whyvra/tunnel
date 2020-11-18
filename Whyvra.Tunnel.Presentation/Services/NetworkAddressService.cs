using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Whyvra.Tunnel.Common.Commands;
using Whyvra.Tunnel.Common.Models;
using Whyvra.Tunnel.Presentation.Configuration;

namespace Whyvra.Tunnel.Presentation.Services
{
    public  class NetworkAddressService
    {
        private readonly HttpClient _http;
        private readonly ApiOptions _api;

        public NetworkAddressService(IHttpClientFactory factory, IOptions<ApiOptions> api)
        {
            _http = factory.CreateClient("TunnelApi");
            _api = api.Value;
        }

        public async Task<int> CreateNew(CreateNetworkAddressCommand command)
        {
            var result = await _http.PostAsJsonAsync($"{_api.Url}/networkaddress", command);
            result.EnsureSuccessStatusCode();

            var response = await result.Content.ReadFromJsonAsync<IdResponse>();

            return response.Id;
        }

        public async Task<IEnumerable<NetworkAddressDto>> GetAll()
        {
            return await _http.GetFromJsonAsync<IEnumerable<NetworkAddressDto>>($"{_api.Url}/networkaddress");
        }
    }
}