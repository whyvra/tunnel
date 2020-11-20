using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Whyvra.Tunnel.Common.Queries;

namespace Whyvra.Tunnel.Api.Controllers
{
    [Route("api/[controller]")]
    public class ClientsController : BaseController
    {
        /// <summary>
        /// Get a WireGuard client
        /// </summary>
        /// <response code="201">Created</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetTask(int id, CancellationToken cancellationToken)
        {
            var query = new GetClientByIdQuery {Id = id};
            var client = await Mediator.Send(query, cancellationToken);

            return new JsonResult(client);
        }
    }
}