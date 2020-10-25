using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Whyvra.Tunnel.Domain.Entitites;

namespace Whyvra.Tunnel.Data.Configuration
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyDataFixForSqlite(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().Property(x => x.Data)
                .HasConversion(
                    x => x.ToJsonString(),
                    x => JsonDocument.Parse(x, new JsonDocumentOptions())
                );

                modelBuilder.Entity<NetworkAddress>().Property(x => x.Address)
                .HasConversion(
                    x => $"{x.addr}/{x.cidr}",
                    x => x.ToAddress()
                );

            modelBuilder.Entity<WireguardClient>().Property(x => x.AssignedIp)
                .HasConversion(
                    x => $"{x.addr}/{x.cidr}",
                    x => x.ToAddress()
                );

            modelBuilder.Entity<WireguardServer>().Property(x => x.AssignedRange)
                .HasConversion(
                    x => $"{x.addr}/{x.cidr}",
                    x => x.ToAddress()
                );

            modelBuilder.Entity<WireguardServer>().Property(x => x.Dns)
                .HasConversion(
                    x => x.ToString(),
                    x => IPAddress.Parse(x)
                );
        }

        public static (IPAddress addr, int cidr) ToAddress(this string address)
        {
            var chunks = address.Split('/');
            return (IPAddress.Parse(chunks[0]), int.Parse(chunks[1]));
        }

        public static string ToJsonString(this JsonDocument json)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = false });
                json.WriteTo(writer);
                writer.Flush();
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}
