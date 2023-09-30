using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Whyvra.Tunnel.Domain.Entities;

namespace Whyvra.Tunnel.Data.Configuration
{
    public class ServerConfiguration : IEntityTypeConfiguration<Server>
    {
        public void Configure(EntityTypeBuilder<Server> builder)
        {
            builder.Property(x => x.Id)
                .HasColumnOrder(1);

            builder.Property(x => x.Name)
                .HasMaxLength(64)
                .IsRequired()
                .HasColumnOrder(2);

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.Property(x => x.Description)
                .HasMaxLength(128)
                .HasColumnOrder(3);

            builder.Property(x => x.AssignedRange)
                .IsRequired()
                .HasColumnOrder(4);

            builder.Property(x => x.Dns)
                .IsRequired()
                .HasColumnOrder(5);

            builder.Property(x => x.Endpoint)
                .IsRequired()
                .HasColumnOrder(6);

            builder.Property(x => x.ListenPort)
                .IsRequired()
                .HasColumnOrder(7);

            builder.Property(x => x.PublicKey)
                .HasMaxLength(44)
                .IsRequired()
                .HasColumnOrder(8);

            builder.Property(x => x.StatusApi)
                .HasMaxLength(128)
                .HasColumnOrder(9);

            builder.Property(x => x.RenderToDisk)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnOrder(10);

            builder.Property(x => x.AddFirewallRules)
                .IsRequired()
                .HasDefaultValue(true)
                .HasColumnOrder(11);

            builder.Property(x => x.NetworkInterface)
                .HasColumnOrder(12)
                .HasDefaultValue("eth0")
                .HasMaxLength(16)
                .IsRequired();

            builder.Property(x => x.CustomConfiguration)
                .HasColumnOrder(13);

            builder.Property(x => x.WireGuardInterface)
                .HasColumnOrder(14)
                .HasMaxLength(16);

            builder.HasIndex(x => x.WireGuardInterface)
                .IsUnique();

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnOrder(15);

            builder.Property(x => x.UpdatedAt)
                .IsRequired()
                .HasColumnOrder(16);

            builder.HasMany(x => x.Clients)
                .WithOne(x => x.Server)
                .HasForeignKey(x => x.ServerId);
        }
    }
}