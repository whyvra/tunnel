using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Whyvra.Tunnel.Common.Commands;
using Whyvra.Tunnel.Core.Servers.Queries;

namespace Whyvra.Tunnel.Api.Controllers
{
    [Route("api/[controller]")]
    public class ServersController : BaseController
    {
        /// <summary>
        /// Get a list of all WireGuard servers
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var query = new GetAllServersQuery();
            var servers = await Mediator.Send(query, cancellationToken);

            return new JsonResult(servers);
        }

        /// <summary>
        /// Get a WireGuard server with specific ID
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var query = new GetServerByIdQuery {Id = id};
            var server = await Mediator.Send(query, cancellationToken);

            return new JsonResult(server);
        }

        /// <summary>
        /// Create a new WireGuard server
        /// </summary>
        /// <response code="201">Created</response>
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Post([FromBody] CreateServerCommand command, CancellationToken cancellationToken)
        {
            var id = await Mediator.Send(command, cancellationToken);

            return new JsonResult(new {id})
            {
                StatusCode = 201
            };
        }
    }
}