using Microsoft.EntityFrameworkCore;
using StudentCardAssignment.Application.Cards.Queries.Common;
using StudentCardAssignment.Application.Students.Queries.Common;

namespace StudentCardAssignment.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    // Read Models (Projections)
    DbSet<StudentReadModel> StudentReadModels { get; }
    DbSet<CardReadModel> CardReadModels { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
