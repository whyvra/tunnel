using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Whyvra.Tunnel.Common.Json
{
    public class IpAddressJsonConverter : JsonConverter<IPAddress>
    {
        public override IPAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => IPAddress.Parse(reader.GetString());

        public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}