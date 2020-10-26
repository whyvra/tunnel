using Microsoft.AspNetCore.Routing;

namespace Whyvra.Tunnel.Api
{
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            return value?.ToString()?.ToLower();
        }
    }
}