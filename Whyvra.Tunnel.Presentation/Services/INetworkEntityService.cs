using System.Threading.Tasks;

namespace Whyvra.Tunnel.Presentation.Services
{
    public interface INetworkEntityService
    {
        Task AddNetworkAddress(int entityId, int networkAddressId);

        Task RemoveNetworkAddress(int entityId, int networkAddressId);
    }
}