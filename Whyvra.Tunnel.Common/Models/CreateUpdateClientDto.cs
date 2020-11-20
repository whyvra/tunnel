namespace Whyvra.Tunnel.Common.Models
{
    public class CreateUpdateClientDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string AssignedIp { get; set; }

        public bool? IsRevoked { get; set; }

        public string PublicKey { get; set; }
    }
}