using MediatR;
using Whyvra.Tunnel.Common.Models;

namespace Whyvra.Tunnel.Core.Clients.Commands
{
    public class CreateClientCommand : IRequest<int>
    {
        public CreateUpdateClientDto Client { get; set; }

        public int ServerId { get; set; }
    }
}