using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Whyvra.Tunnel.Domain.Entities;

namespace Whyvra.Tunnel.Data.Configuration
{
    public class ClientNetworkAddressConfiguration : IEntityTypeConfiguration<ClientNetworkAddress>
    {
        public void Configure(EntityTypeBuilder<ClientNetworkAddress> builder)
        {
            builder.HasIndex(x => new { x.NetworkAddressId, x.ClientId })
                .IsUnique();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired();

            builder.HasOne(x => x.Client)
                .WithMany(x => x.AllowedIps)
                .HasForeignKey(x => x.ClientId)
                .IsRequired();

            builder.HasOne(x => x.NetworkAddress)
                .WithMany()
                .HasForeignKey(x => x.NetworkAddressId)
                .IsRequired();
        }
    }
}