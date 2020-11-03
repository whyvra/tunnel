using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Whyvra.Tunnel.Presentation.Authentication
{
    public class CustomAuthorizationMessageHandler : BaseAddressAuthorizationMessageHandler
    {
        public CustomAuthorizationMessageHandler(IdTokenProvider idTokenProvider, NavigationManager navigationManager) : base(idTokenProvider, navigationManager)
        {
        }
    }
}