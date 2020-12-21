namespace Whyvra.Tunnel.Presentation.Configuration
{
    public class AuthenticationOptions
    {
        public string Authority { get; set; }

        public string ClientId { get; set; }

        public bool Enabled { get; set; }

        public string RequiredRole { get; set; }
    }
}