using System;
using System.Net;

namespace Whyvra.Tunnel.Domain.Entities
{
    public class NetworkAddress : IEntity
    {
        public int Id { get; set; }

        public (IPAddress addr, int cidr) Address { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}