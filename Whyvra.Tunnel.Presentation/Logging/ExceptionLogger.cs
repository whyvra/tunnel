using System;
using Microsoft.Extensions.Logging;

namespace Whyvra.Tunnel.Presentation.Logging
{
    public class ExceptionLogger : ILogger
    {
        private readonly IExceptionHandler _handler;

        public ExceptionLogger(IExceptionHandler handler)
        {
            _handler = handler;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new NoopDisposable();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (exception == null) return;
            _handler.Handle(exception);
        }

        private class NoopDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}