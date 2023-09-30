using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Whyvra.Tunnel.Domain.Entities;

namespace Whyvra.Tunnel.Data.Configuration
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.Property(x => x.EventType)
                .HasMaxLength(16)
                .IsRequired();

            builder.Property(x => x.SourceAddress)
                .HasMaxLength(45)
                .IsRequired();

            builder.Property(x => x.Timestamp)
                .IsRequired();

            builder.Property(x => x.TableId)
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(x => x.RecordId)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .IsRequired();
        }
    }
}