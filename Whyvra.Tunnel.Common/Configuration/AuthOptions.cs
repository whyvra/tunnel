namespace Whyvra.Tunnel.Common.Configuration
{
    public class AuthOptions
    {
        public string Authority { get; set; }

        public string ClientId { get; set; }

        public bool Enabled { get; set; }

        public string RequiredRole { get; set; }

        public string ResponseType { get; set; } = "code";
    }
}