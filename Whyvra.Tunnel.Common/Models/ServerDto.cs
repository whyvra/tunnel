using System;
using System.Collections.Generic;
using System.Linq;

namespace Whyvra.Tunnel.Common.Models
{
    public class ServerDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string AssignedRange { get; set; }

        public IEnumerable<ClientDto> Clients { get; set; }

        public IEnumerable<string> DefaultAllowedRange { get; set; } = Enumerable.Empty<string>();

        public string Dns { get; set; }

        public string Endpoint { get; set; }

        public int ListenPort { get; set; }

        public string PublicKey { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}