using System;
using System.Collections.Generic;

namespace Whyvra.Tunnel.Common.Models
{
    public class ClientDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> AllowedIps { get ; set; }

        public string AssignedIp { get; set; }

        public bool? IsRevoked { get; set; }

        public string PublicKey { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}