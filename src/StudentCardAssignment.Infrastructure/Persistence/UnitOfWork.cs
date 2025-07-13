using StudentCardAssignment.Application.Common.Interfaces;
using StudentCardAssignment.Infrastructure.Persistence;
using StudentCardAssignment.Infrastructure.EventStore;
using StudentCardAssignment.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace StudentCardAssignment.Infrastructure.Persistence;

public class UnitOfWork(ApplicationDbContext context, IEventStore eventStore) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;
    private readonly IEventStore _eventStore = eventStore;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entitiesWithEvents = _context.ChangeTracker.Entries<AggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = entitiesWithEvents
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        foreach (var entityEntry in entitiesWithEvents)
        {
            var entity = entityEntry.Entity;
            if (entity.DomainEvents.Any())
            {
                await _eventStore.SaveEventsAsync(
                    entity.Id,
                    entity.GetType().Name,
                    entity.DomainEvents,
                    0, // For simplicity, 0 is used as expected version. In real scenarios, this shd be track versions properly
                    cancellationToken);

                entity.ClearDomainEvents();
            }
        }

        // Save changes to main database
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
