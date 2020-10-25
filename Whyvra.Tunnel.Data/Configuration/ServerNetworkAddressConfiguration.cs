using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Whyvra.Tunnel.Domain.Entitites;

namespace Whyvra.Tunnel.Data.Configuration
{
    public class ServerNetworkAddressConfiguration : IEntityTypeConfiguration<ServerNetworkAddress>
    {
        public void Configure(EntityTypeBuilder<ServerNetworkAddress> builder)
        {
            builder.HasIndex(x => new { x.NetworkAddressId, x.ServerId })
                .IsUnique();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired();

            builder.HasOne(x => x.NetworkAddress)
                .WithMany()
                .HasForeignKey(x => x.NetworkAddressId)
                .IsRequired();

            builder.HasOne(x => x.Server)
                .WithMany(x => x.DefaultAllowedRange)
                .HasForeignKey(x => x.ServerId)
                .IsRequired();
        }
    }
}