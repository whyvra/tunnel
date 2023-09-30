using System;
using System.Collections.Generic;
using System.Net;

namespace Whyvra.Tunnel.Domain.Entities
{
    public class Server : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public (IPAddress addr, int cidr) AssignedRange { get; set; }

        public IEnumerable<ServerNetworkAddress> DefaultAllowedRange { get; set; }

        public IPAddress Dns { get; set; }

        public string Endpoint { get; set; }

        public int ListenPort { get; set; }

        public string PublicKey { get; set; }

        public string StatusApi { get; set; }

        public bool RenderToDisk { get; set; }

        public bool? AddFirewallRules { get; set; }

        public string NetworkInterface { get; set; }

        public string CustomConfiguration { get; set; }

        public string WireGuardInterface { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public IEnumerable<Client> Clients { get; set; }
    }
}