using MediatR;

namespace Whyvra.Tunnel.Common.Commands
{
    public class CreateServerCommand : IRequest<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string AssignedRange { get; set; }

        public string Dns { get; set; }

        public string Endpoint { get; set; }

        public string PublicKey { get; set; }
    }
}