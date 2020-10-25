using System;

namespace Whyvra.Tunnel.Domain.Entitites
{
    public class ServerNetworkAddress : IEntity
    {
        public int Id { get; set; }

        public int NetworkAddressId { get; set; }

        public NetworkAddress NetworkAddress { get; set; }

        public int ServerId { get; set; }

        public WireguardServer Server { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}