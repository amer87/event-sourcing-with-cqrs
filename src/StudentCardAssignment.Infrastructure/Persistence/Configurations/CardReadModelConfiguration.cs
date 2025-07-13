using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentCardAssignment.Application.Cards.Queries.Common;

namespace StudentCardAssignment.Infrastructure.Persistence.Configurations;

public class CardReadModelConfiguration : IEntityTypeConfiguration<CardReadModel>
{
    public void Configure(EntityTypeBuilder<CardReadModel> builder)
    {
        builder.ToTable("CardReadModels");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CardId)
            .IsRequired();

        builder.HasIndex(c => c.CardId)
            .IsUnique();

        builder.Property(c => c.CardNumber)
            .HasMaxLength(16)
            .IsRequired();

        builder.HasIndex(c => c.CardNumber)
            .IsUnique();

        builder.Property(c => c.MaskedCardNumber)
            .HasMaxLength(20);

        builder.Property(c => c.CardType)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.Status)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.AssignedStudentName)
            .HasMaxLength(100);

        builder.Property(c => c.AssignedStudentEmail)
            .HasMaxLength(100);

        builder.Property(c => c.IssuedAt)
            .IsRequired();

        builder.Property(c => c.ExpiresAt)
            .IsRequired();

        // Indexes for common queries
        builder.HasIndex(c => c.IsAssigned);
        builder.HasIndex(c => c.IsActive);
        builder.HasIndex(c => c.Status);
        builder.HasIndex(c => c.AssignedStudentId);
        builder.HasIndex(c => c.ExpiresAt);
        builder.HasIndex(c => c.CardType);
    }
}
