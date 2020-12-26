using System;
using Whyvra.Tunnel.Common.Models;

namespace Whyvra.Tunnel.Presentation.Configuration
{
    public class TunnelException : Exception
    {
        private readonly string _formattedMessage;

        public TunnelException() : base()
        {
        }

        public TunnelException(string message) : base(message)
        {
        }

        public TunnelException(string message, string formattedMessage, ApiMessage apiMessage) : base(message)
        {
            ApiMessage = apiMessage;
            _formattedMessage = formattedMessage;
        }

        public TunnelException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ApiMessage ApiMessage { get; }

        public string FormattedMessage => _formattedMessage ?? Message;
    }
}