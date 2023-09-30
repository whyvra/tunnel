using System;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Whyvra.Tunnel.Domain.Entities;

namespace Whyvra.Tunnel.Data.Configuration
{
    public class NetworkAddressConfiguration : IEntityTypeConfiguration<NetworkAddress>
    {
        public void Configure(EntityTypeBuilder<NetworkAddress> builder)
        {
            builder.Property(x => x.Address)
                .IsRequired();

            builder.HasIndex(x => x.Address)
                .IsUnique();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired();

            var now = DateTime.UtcNow;

            builder.HasData(new [] {
                new NetworkAddress {Id = 1, Address = (IPAddress.Parse("0.0.0.0"), 0), CreatedAt = now, UpdatedAt = now},
                new NetworkAddress {Id = 2, Address = (IPAddress.Parse("::"), 0), CreatedAt = now, UpdatedAt = now}
            });
        }
    }
}