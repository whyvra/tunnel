namespace Whyvra.Tunnel.Common.Models
{
    public class ApiMessage
    {
        public string InnerException { get; set; }

        public string Message { get; set; }

        public string Status { get; set; }

        public int StatusCode { get; set; }
    }
}