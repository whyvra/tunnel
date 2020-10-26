using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Whyvra.Tunnel.Data;
using Whyvra.Tunnel.Domain.Entitites;

namespace Whyvra.Tunnel.Core.Users.Commands
{
    public class CreateUpdateUserCommandHandler : IRequestHandler<CreateUpdateUserCommand, int>
    {
        private readonly ITunnelContext _context;
        private readonly IUserManager _userManager;

        public CreateUpdateUserCommandHandler(ITunnelContext context, IUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<int> Handle(CreateUpdateUserCommand request, CancellationToken cancellationToken)
        {
            var uid = _userManager.GetUserUid();
            var claims = request.Claims;

            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.Uid.Equals(uid), cancellationToken);

            if (user == null)
            {
                // Create new user
                user = new User
                {
                    Email = claims[ClaimTypes.Email],
                    FirstName = claims[ClaimTypes.GivenName],
                    LastName = claims[ClaimTypes.Surname],
                    Uid = uid,
                    Username = claims["preferred_username"]
                };

                await _context.Users.AddAsync(user, cancellationToken);
            }
            else
            {
                // Try to update user entry
                user.Email = claims[ClaimTypes.Email];
                user.FirstName = claims[ClaimTypes.GivenName];
                user.LastName = claims[ClaimTypes.Surname];
                user.Username = claims["preferred_username"];
            }

            await _context.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}
