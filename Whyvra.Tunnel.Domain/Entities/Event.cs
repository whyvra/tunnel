using System;
using System.Text.Json;

namespace Whyvra.Tunnel.Domain.Entities
{
    public class Event
    {
        public int Id { get; set; }

        public JsonDocument Data { get; set; }

        public string EventType { get; set; }

        public string SourceAddress { get; set; }

        public DateTime Timestamp { get; set; }

        public string TableId { get; set; }

        public int RecordId { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}