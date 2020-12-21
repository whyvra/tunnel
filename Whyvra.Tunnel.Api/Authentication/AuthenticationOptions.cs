namespace Whyvra.Tunnel.Api.Authentication
{
    public class AuthenticationOptions
    {
        public string Audience { get; set; }

        public bool Enabled { get; set; }

        public string Issuer { get; set; }

        public string RequiredRole { get; set; }

        public string WellKnownEndpoint { get; set; }
    }
}