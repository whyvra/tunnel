using System;

namespace Whyvra.Tunnel.Domain.Entities
{
    public class ServerNetworkAddress : IEntity
    {
        public int Id { get; set; }

        public int NetworkAddressId { get; set; }

        public NetworkAddress NetworkAddress { get; set; }

        public int ServerId { get; set; }

        public Server Server { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}