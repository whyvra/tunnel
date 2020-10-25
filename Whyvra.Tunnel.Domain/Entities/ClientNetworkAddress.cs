using System;

namespace Whyvra.Tunnel.Domain.Entitites
{
    public class ClientNetworkAddress : IEntity
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public WireguardClient Client { get; set; }

        public int NetworkAddressId { get; set; }

        public NetworkAddress NetworkAddress { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}