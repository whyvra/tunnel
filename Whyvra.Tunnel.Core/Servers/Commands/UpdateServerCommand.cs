using MediatR;
using Whyvra.Tunnel.Common.Models;

namespace Whyvra.Tunnel.Core.Servers.Commands
{
    public class UpdateServerCommand : IRequest
    {
        public int Id { get; set; }

        public CreateUpdateServerDto Data { get; set; }
    }
}