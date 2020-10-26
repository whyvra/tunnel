using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Whyvra.Tunnel.Core.Users.Commands;

namespace Whyvra.Tunnel.Api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        /// <summary>
        /// Create or update a user
        /// </summary>
        /// <response code="201">Created</response>
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Post(CancellationToken cancellationToken)
        {
            var claims = HttpContext.User.Claims.ToDictionary(x => x.Type, x => x.Value);
            var command = new CreateUpdateUserCommand {Claims = claims};
            var id = await Mediator.Send(command, cancellationToken);

            return new JsonResult(new {id})
            {
                StatusCode = 201
            };
        }
    }
}