using System;

namespace Whyvra.Tunnel.Presentation.Logging
{
    public class ExceptionHandler : IExceptionHandler
    {
        public event EventHandler<Exception> OnUnhandledException;

        public void Handle(Exception e)
        {
            OnUnhandledException?.Invoke(this, e);
        }
    }
}