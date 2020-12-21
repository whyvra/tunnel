using System.Collections.Generic;

namespace Whyvra.Tunnel.Presentation.ViewModels
{
    public interface IClientViewModel<TModel>
    {
        TModel Client { get; set; }

        IEnumerable<string> AllowedIps { get; set; }
    }
}