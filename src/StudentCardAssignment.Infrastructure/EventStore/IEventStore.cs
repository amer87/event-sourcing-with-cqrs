using StudentCardAssignment.Domain.Common;

namespace StudentCardAssignment.Infrastructure.EventStore;

public interface IEventStore
{
    Task SaveEventsAsync(Guid aggregateId, string aggregateType, IEnumerable<IDomainEvent> events, long expectedVersion, CancellationToken cancellationToken = default);
    Task<IEnumerable<IDomainEvent>> GetEventsAsync(Guid aggregateId, CancellationToken cancellationToken = default);
    Task<IEnumerable<IDomainEvent>> GetEventsAsync(Guid aggregateId, long fromVersion, CancellationToken cancellationToken = default);
    Task<IEnumerable<EventStoreRecord>> GetAllEventsAsync(CancellationToken cancellationToken = default);
}
