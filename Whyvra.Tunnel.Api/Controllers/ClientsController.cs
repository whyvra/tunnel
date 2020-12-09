using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Whyvra.Tunnel.Common.Models;
using Whyvra.Tunnel.Common.Queries;
using Whyvra.Tunnel.Core.Clients.Commands;

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
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var query = new GetClientByIdQuery {Id = id};
            var client = await Mediator.Send(query, cancellationToken);

            return new JsonResult(client);
        }

        /// <summary>
        /// Update a WireGuard client
        /// </summary>
        /// <response code="204">NoContent</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateClientDto client, CancellationToken cancellationToken)
        {
            var command = new UpdateClientCommand {Id = id, Client = client};
            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Add a network address to a WireGuard client's allowed IPs
        /// </summary>
        /// <response code="201">Created</response>
        [HttpPut("{id}/allowedips/{networkAddressId}")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Put(int id, int networkAddressId, CancellationToken cancellationToken)
        {
            var command = new AddAddressToClientCommand {ClientId = id, NetworkAddressId = networkAddressId};
            var result = await Mediator.Send(command, cancellationToken);

            return new JsonResult(new {id = result})
            {
                StatusCode = 201
            };
        }

        /// <summary>
        /// Remove a network address from a WireGuard client's allowed IPs
        /// </summary>
        /// <response code="204">NoContent</response>
        [HttpDelete("{id}/allowedips/{networkAddressId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id, int networkAddressId, CancellationToken cancellationToken)
        {
            var command = new RemoveAddressFromClientCommand {ClientId = id, NetworkAddressId = networkAddressId};
            await Mediator.Send(command, cancellationToken);

            return NoContent();
        }
    }
}