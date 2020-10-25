using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Whyvra.Tunnel.Domain.Entitites;

namespace Whyvra.Tunnel.Data.Configuration
{
    public class WireguardClientConfiguration : IEntityTypeConfiguration<WireguardClient>
    {
        public void Configure(EntityTypeBuilder<WireguardClient> builder)
        {
            builder.Property(x => x.Name)
                .HasMaxLength(64)
                .IsRequired();

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.Property(x => x.Description)
                .HasMaxLength(128);

            builder.Property(x => x.AssignedIp)
                .IsRequired();

            builder.HasIndex(x => x.AssignedIp)
                .IsUnique();

            builder.Property(x => x.PublicKey)
                .HasMaxLength(44)
                .IsRequired();

            builder.HasOne(x => x.Server)
                .WithMany(x => x.Clients)
                .HasForeignKey(x => x.ServerId);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired();

            builder.HasMany(x => x.AllowedIps)
                .WithOne(x => x.Client);
        }
    }
}