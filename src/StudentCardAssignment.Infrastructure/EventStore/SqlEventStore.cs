using Microsoft.EntityFrameworkCore;
using StudentCardAssignment.Domain.Common;
using StudentCardAssignment.Infrastructure.Persistence;
using System.Text.Json;

namespace StudentCardAssignment.Infrastructure.EventStore;

public class SqlEventStore(ApplicationDbContext context) : IEventStore
{
    private readonly ApplicationDbContext _context = context;

    public async Task SaveEventsAsync(Guid aggregateId, string aggregateType, IEnumerable<IDomainEvent> events, long expectedVersion, CancellationToken cancellationToken = default)
    {
        var eventsList = events.ToList();
        if (!eventsList.Any()) return;

        // Check for concurrency conflicts
        var currentVersion = await GetCurrentVersionAsync(aggregateId, cancellationToken);
        if (currentVersion != expectedVersion)
        {
            throw new InvalidOperationException($"Concurrency conflict. Expected version {expectedVersion}, but current version is {currentVersion}");
        }

        var eventRecords = new List<EventStoreRecord>();
        long version = expectedVersion;

        foreach (var domainEvent in eventsList)
        {
            version++;
            var eventRecord = new EventStoreRecord
            {
                Id = Guid.NewGuid(),
                AggregateId = aggregateId,
                AggregateType = aggregateType,
                EventType = domainEvent.GetType().Name,
                EventData = JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
                Metadata = JsonSerializer.Serialize(new
                {
                    EventId = domainEvent.Id,
                    domainEvent.OccurredOn,
                    EventVersion = "1.0"
                }),
                OccurredOn = domainEvent.OccurredOn,
                Version = version,
                CreatedAt = DateTime.UtcNow
            };

            eventRecords.Add(eventRecord);
        }

        await _context.EventStore.AddRangeAsync(eventRecords, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<IDomainEvent>> GetEventsAsync(Guid aggregateId, CancellationToken cancellationToken = default)
    {
        return await GetEventsAsync(aggregateId, 0, cancellationToken);
    }

    public async Task<IEnumerable<IDomainEvent>> GetEventsAsync(Guid aggregateId, long fromVersion, CancellationToken cancellationToken = default)
    {
        var eventRecords = await _context.EventStore
            .Where(e => e.AggregateId == aggregateId && e.Version > fromVersion)
            .OrderBy(e => e.Version)
            .ToListAsync(cancellationToken);

        var domainEvents = new List<IDomainEvent>();

        foreach (var eventRecord in eventRecords)
        {
            var eventType = GetEventType(eventRecord.EventType);
            if (eventType != null &&
                JsonSerializer.Deserialize(eventRecord.EventData, eventType) is IDomainEvent domainEvent
            )
            {
                domainEvents.Add(domainEvent);
            }
        }

        return domainEvents;
    }

    public async Task<IEnumerable<EventStoreRecord>> GetAllEventsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.EventStore
            .OrderBy(e => e.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    private async Task<long> GetCurrentVersionAsync(Guid aggregateId, CancellationToken cancellationToken)
    {
        var lastEvent = await _context.EventStore
            .Where(e => e.AggregateId == aggregateId)
            .OrderByDescending(e => e.Version)
            .FirstOrDefaultAsync(cancellationToken);

        return lastEvent?.Version ?? 0;
    }

    private static Type? GetEventType(string eventTypeName)
    {
        // This is a simple implementation. In a real-world scenario, you might want to use a more sophisticated type resolution strategy
        var assemblies = new[]
        {
            typeof(StudentCardAssignment.Domain.Students.Events.StudentCreatedDomainEvent).Assembly,
            typeof(StudentCardAssignment.Domain.Cards.Events.CardCreatedDomainEvent).Assembly
        };

        foreach (var assembly in assemblies)
        {
            var type = assembly.GetTypes()
                .FirstOrDefault(t => t.Name == eventTypeName && typeof(IDomainEvent).IsAssignableFrom(t));

            if (type != null)
                return type;
        }

        return null;
    }
}
