using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Whyvra.Tunnel.Common.Json
{
    public class CidrAddressJsonConverter : JsonConverter<(IPAddress addr, int cidr)>
    {
        public override (IPAddress addr, int cidr) Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var json = reader.GetString();
            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            var addr = IPAddress.Parse(root.GetProperty("addr").GetString());
            return (addr, root.GetProperty("cidr").GetInt32());
        }

        public override void Write(Utf8JsonWriter writer, (IPAddress addr, int cidr) value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("addr", value.addr.ToString());
            writer.WriteNumber("cidr", value.cidr);
            writer.WriteEndObject();
        }
    }
}