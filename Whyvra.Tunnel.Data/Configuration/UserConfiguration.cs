using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Whyvra.Tunnel.Domain.Entitites;

namespace Whyvra.Tunnel.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Email)
                .HasMaxLength(128)
                .IsRequired();

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.FirstName)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.Username)
                .HasMaxLength(128)
                .IsRequired();

            builder.HasIndex(x => x.Username)
                .IsUnique();

            builder.Property(x => x.Uid)
                .IsRequired();

            builder.HasIndex(x => x.Uid)
                .IsUnique();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired();

            // Create default user for event tracking if authentication is disabled
            var now = DateTime.UtcNow;
            builder.HasData(new User
            {
                Id = 1,
                Email = "system@example.com",
                FirstName = "System",
                LastName = "User",
                Username = "system_user",
                Uid = Guid.Parse("e3adf55b-7430-42c1-ae62-758d7b644fdb"),
                CreatedAt = now,
                UpdatedAt = now
            });
        }
    }
}