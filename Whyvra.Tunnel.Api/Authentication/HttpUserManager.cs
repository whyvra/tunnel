using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Whyvra.Tunnel.Core.Users;

namespace Whyvra.Tunnel.Api.Authentication
{
    public class HttpUserManager : IUserManager
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly AuthenticationOptions _opts;

        public HttpUserManager(IHttpContextAccessor accessor, IOptions<AuthenticationOptions> opts)
        {
            _accessor = accessor;
            _opts = opts.Value;
        }

        public string GetSourceAddress()
        {
            return _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        public Guid GetUserUid()
        {
            if (_opts.Enabled)
            {
                var uid = _accessor.HttpContext.User.Claims.SingleOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
                return uid == null ? Guid.Empty : Guid.Parse(uid);
            }

            return Guid.Parse("E3ADF55B-7430-42C1-AE62-758D7B644FDB");
        }
    }
}