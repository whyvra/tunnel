using System;
using Microsoft.Extensions.Logging;

namespace Whyvra.Tunnel.Presentation.Logging
{
    public class ExceptionLoggerProvider : ILoggerProvider
    {
        private readonly IExceptionHandler _handler;

        public ExceptionLoggerProvider(IExceptionHandler handler)
        {
            _handler = handler;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new ExceptionLogger(_handler);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}