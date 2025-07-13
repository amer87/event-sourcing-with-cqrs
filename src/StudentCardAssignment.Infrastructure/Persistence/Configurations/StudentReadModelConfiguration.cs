using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentCardAssignment.Application.Students.Queries.Common;

namespace StudentCardAssignment.Infrastructure.Persistence.Configurations;

public class StudentReadModelConfiguration : IEntityTypeConfiguration<StudentReadModel>
{
    public void Configure(EntityTypeBuilder<StudentReadModel> builder)
    {
        builder.ToTable("StudentReadModels");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.StudentId)
            .IsRequired();

        builder.HasIndex(s => s.StudentId)
            .IsUnique();

        builder.Property(s => s.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(s => s.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(s => s.FullName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(s => s.Email)
            .IsUnique();

        builder.Property(s => s.StudentNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(s => s.StudentNumber)
            .IsUnique();

        builder.Property(s => s.Status)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(s => s.DisplayName)
            .HasMaxLength(150);

        builder.Property(s => s.IsActive)
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        // Card assignment properties
        builder.Property(s => s.HasAssignedCard)
            .IsRequired();

        builder.Property(s => s.AssignedCardId);

        builder.Property(s => s.AssignedCardNumber)
            .HasMaxLength(20);

        builder.Property(s => s.AssignedAt);

        // Indexes for common queries
        builder.HasIndex(s => s.IsActive);
        builder.HasIndex(s => s.Status);
        builder.HasIndex(s => new { s.LastName, s.FirstName });
        builder.HasIndex(s => s.HasAssignedCard);
    }
}
