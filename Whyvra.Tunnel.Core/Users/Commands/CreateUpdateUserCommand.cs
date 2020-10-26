using System.Collections.Generic;
using MediatR;

namespace Whyvra.Tunnel.Core.Users.Commands
{
    public class CreateUpdateUserCommand : IRequest<int>
    {
        public IDictionary<string, string> Claims { get; set; }
    }
}
