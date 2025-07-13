using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentCardAssignment.Infrastructure.EventStore;

namespace StudentCardAssignment.Infrastructure.Persistence.Configurations;

public class EventStoreConfiguration : IEntityTypeConfiguration<EventStoreRecord>
{
    public void Configure(EntityTypeBuilder<EventStoreRecord> builder)
    {
        builder.ToTable("EventStore");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.AggregateId)
            .IsRequired();

        builder.Property(e => e.AggregateType)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.EventType)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.EventData)
            .IsRequired();

        builder.Property(e => e.Metadata);

        builder.Property(e => e.OccurredOn)
            .IsRequired();

        builder.Property(e => e.Version)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        // Create indexes for better query performance
        builder.HasIndex(e => e.AggregateId);
        builder.HasIndex(e => new { e.AggregateId, e.Version }).IsUnique();
        builder.HasIndex(e => e.CreatedAt);
        builder.HasIndex(e => e.EventType);
    }
}
