using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Whyvra.Tunnel.Api.Controllers
{
    [Route("api/[controller]")]
    public class VersionController
    {
        /// <summary>
        /// Get current version
        /// </summary>
        /// <response code="200">OK</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult Get()
        {
            return new JsonResult(new { version = Assembly.GetExecutingAssembly().GetName().Version.ToString() });
        }
    }
}