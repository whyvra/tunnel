using MediatR;
using Whyvra.Tunnel.Common.Models;

namespace Whyvra.Tunnel.Core.Clients.Commands
{
    public class UpdateClientCommand : IRequest
    {
        public int Id { get; set; }

        public UpdateClientDto Client { get; set; }
    }
}