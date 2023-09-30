using System;
using System.Collections.Generic;
using System.Net;

namespace Whyvra.Tunnel.Domain.Entitites
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

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public IEnumerable<Client> Clients { get; set; }
    }
}