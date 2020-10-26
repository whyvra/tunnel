using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Whyvra.Tunnel.Api.Controllers;

namespace Whyvra.Tunnel.Api.Authentication
{
    public class AuthenticationFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly bool _isAuthenticationEnabled;

        public AuthenticationFeatureProvider(bool isAuthenticationEnabled)
        {
            _isAuthenticationEnabled = isAuthenticationEnabled;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            if (_isAuthenticationEnabled) return;

            var users = feature.Controllers.SingleOrDefault(x =>
                x.Name.Equals(nameof(UsersController), StringComparison.InvariantCultureIgnoreCase));

            if (users != null)
            {
                feature.Controllers.Remove(users);
            }
        }
    }
}