using System;

namespace Whyvra.Tunnel.Domain.Entities
{
    public class ClientNetworkAddress : IEntity
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }

        public int NetworkAddressId { get; set; }

        public NetworkAddress NetworkAddress { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}