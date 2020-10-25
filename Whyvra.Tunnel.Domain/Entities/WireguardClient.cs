using System;
using System.Collections.Generic;
using System.Net;

namespace Whyvra.Tunnel.Domain.Entitites
{
    public class WireguardClient : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public (IPAddress addr, int cidr) AssignedIp { get; set; }

        public string PublicKey { get; set; }

        public int ServerId { get; set; }

        public WireguardServer Server { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public IEnumerable<ClientNetworkAddress> AllowedIps { get; set; }
    }
}