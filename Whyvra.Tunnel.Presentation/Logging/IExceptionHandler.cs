using System;

namespace Whyvra.Tunnel.Presentation.Logging
{
    public interface IExceptionHandler
    {
        event EventHandler<Exception> OnUnhandledException;

        void Handle(Exception e);
    }
}