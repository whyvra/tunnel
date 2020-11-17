using MediatR;

namespace Whyvra.Tunnel.Common.Commands
{
    public class CreateNetworkAddressCommand : IRequest<int>
    {
        public string Address { get; set; }
    }
}