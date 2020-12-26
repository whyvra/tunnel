using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
            await result.EnsureSuccess();

            var response = await result.Content.ReadFromJsonAsync<IdResponse>();

            return response.Id;
        }

        public async Task<IEnumerable<NetworkAddressDto>> GetAll()
        {
            return await _http.GetFromJsonAsyncWithSuccess<IEnumerable<NetworkAddressDto>>($"{_api.Url}/networkaddress");
        }

        public async Task ProcessNetworkAddresses(INetworkEntityService service, int entityId, IEnumerable<string> newRange, IEnumerable<string> oldRange)
        {
            if (!newRange.SequenceEqual(oldRange))
            {
                // Figure out what needs to be added or removed
                var toAdd = newRange.Except(oldRange);
                var toRemove = oldRange.Except(newRange);

                // Get all existing network addresses
                var addresses = await GetAll();

                // Process address to add
                foreach(var addr in toAdd)
                {
                    if (addresses.Any(x => x.Address.Equals(addr)))
                    {
                        // Address already exists so get it's ID and just add it
                        var netId = addresses.Single(x => x.Address.Equals(addr)).Id;
                        await service.AddNetworkAddress(entityId, netId);
                    }
                    else
                    {
                        // Address doesn't exist so create it first and then add it
                        var command = new CreateNetworkAddressCommand { Address = addr };
                        var id = await CreateNew(command);
                        await service.AddNetworkAddress(entityId, id);
                    }
                }

                // Process address to remove from server
                foreach (var addr in toRemove)
                {
                    var netId = addresses.Single(x => x.Address.Equals(addr)).Id;
                    await service.RemoveNetworkAddress(entityId, netId);
                }
            }
        }
    }
}