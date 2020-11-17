using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Whyvra.Tunnel.Common.Commands;
using Whyvra.Tunnel.Core.NetworkAddress.Queries;

namespace Whyvra.Tunnel.Api.Controllers
{
    [Route("api/[controller]")]
    public class NetworkAddressController : BaseController
    {
        /// <summary>
        /// Get a list of all network addresses
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var query = new GetAllNetworkAddressQuery();
            var result = await Mediator.Send(query, cancellationToken);

            return new JsonResult(result);
        }

        /// <summary>
        /// Create a new network address
        /// </summary>
        /// <response code="201">Created</response>
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Post([FromBody] CreateNetworkAddressCommand command, CancellationToken cancellationToken)
        {
            var id = await Mediator.Send(command, cancellationToken);

            return new JsonResult (new {id})
            {
                StatusCode = 201
            };
        }
    }
}