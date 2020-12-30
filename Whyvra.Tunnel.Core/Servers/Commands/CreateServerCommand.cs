using MediatR;
using Whyvra.Tunnel.Common.Models;

namespace Whyvra.Tunnel.Core.Servers.Commands
{
    public class CreateServerCommand : IRequest<int>
    {
        public CreateUpdateServerDto Server { get; set; }
    }
}