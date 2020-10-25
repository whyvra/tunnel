using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Whyvra.Tunnel.Domain.Entitites;

namespace Whyvra.Tunnel.Data.Configuration
{
    public class WireguardServerConfiguration : IEntityTypeConfiguration<WireguardServer>
    {
        public void Configure(EntityTypeBuilder<WireguardServer> builder)
        {
            builder.Property(x => x.Name)
                .HasMaxLength(64)
                .IsRequired();

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.Property(x => x.Description)
                .HasMaxLength(128);

            builder.Property(x => x.AssignedRange)
                .IsRequired();

            builder.Property(x => x.Dns)
                .IsRequired();

            builder.Property(x => x.Endpoint)
                .IsRequired();

            builder.Property(x => x.PublicKey)
                .HasMaxLength(44)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired();

            builder.HasMany(x => x.Clients)
                .WithOne(x => x.Server)
                .HasForeignKey(x => x.ServerId);
        }
    }
}