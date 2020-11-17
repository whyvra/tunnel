using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Whyvra.Tunnel.Common.Models;
using Whyvra.Tunnel.Core.Servers.Commands;
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
        public async Task<IActionResult> Post([FromBody] CreateUpdateServerDto server, CancellationToken cancellationToken)
        {
            var command = new CreateServerCommand {Data = server};
            var id = await Mediator.Send(command, cancellationToken);

            return new JsonResult(new {id})
            {
                StatusCode = 201
            };
        }

        /// <summary>
        /// Update an existing new WireGuard server
        /// </summary>
        /// <response code="204">NoContent</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Put(int id, [FromBody] CreateUpdateServerDto server, CancellationToken cancellationToken)
        {
            var command = new UpdateServerCommand {Id = id, Data = server};
            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Add a network address to a WireGuard server's default allowed range
        /// </summary>
        /// <response code="201">Created</response>
        [HttpPut("{id}/allowedrange/{networkAddressId}")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Put(int id, int networkAddressId, CancellationToken cancellationToken)
        {
            var command = new AddAddressToServerCommand {NetworkAddressId = networkAddressId, ServerId = id};
            var result = await Mediator.Send(command, cancellationToken);

            return new JsonResult(new {id = result})
            {
                StatusCode = 201
            };
        }

        /// <summary>
        /// Remove a network address from a WireGuard server's default allowed range
        /// </summary>
        /// <response code="204">NoContent</response>
        [HttpDelete("{id}/allowedrange/{networkAddressId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id, int networkAddressId, CancellationToken cancellationToken)
        {
            var command = new RemoveAddressFromServerCommand {NetworkAddressId = networkAddressId, ServerId = id};
            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }
    }
}