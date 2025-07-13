using Microsoft.EntityFrameworkCore;
using StudentCardAssignment.Infrastructure.Persistence.Configurations;
using StudentCardAssignment.Infrastructure.EventStore;
using StudentCardAssignment.Application.Students.Queries.Common;
using StudentCardAssignment.Application.Cards.Queries.Common;
using StudentCardAssignment.Application.Common.Interfaces;

namespace StudentCardAssignment.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    // Event Store (Source of Truth)
    public DbSet<EventStoreRecord> EventStore { get; set; } = null!;

    // Read Models (Projections)
    public DbSet<StudentReadModel> StudentReadModels { get; set; } = null!;
    public DbSet<CardReadModel> CardReadModels { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Event Store Configuration
        modelBuilder.ApplyConfiguration(new EventStoreConfiguration());

        // Read Model Configurations (Projections)
        modelBuilder.ApplyConfiguration(new StudentReadModelConfiguration());
        modelBuilder.ApplyConfiguration(new CardReadModelConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
