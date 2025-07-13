using StudentCardAssignment.Application.Common.Interfaces;
using StudentCardAssignment.Domain.Common;
using StudentCardAssignment.Infrastructure.EventStore;
using MediatR;

namespace StudentCardAssignment.Infrastructure.Persistence;

public class EventSourcingUnitOfWork : IUnitOfWork
{
    private readonly IEventStore _eventStore;
    private readonly IMediator _mediator;
    private readonly List<AggregateRoot> _aggregates = new();

    public EventSourcingUnitOfWork(IEventStore eventStore, IMediator mediator)
    {
        _eventStore = eventStore;
        _mediator = mediator;
    }

    public void RegisterAggregate(AggregateRoot aggregate)
    {
        _aggregates.Add(aggregate);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var eventsToPublish = new List<IDomainEvent>();

        foreach (var aggregate in _aggregates)
        {
            var events = aggregate.DomainEvents;
            if (events.Any())
            {
                await _eventStore.SaveEventsAsync(
                    aggregate.Id,
                    aggregate.GetType().Name,
                    events,
                    aggregate.Version,
                    cancellationToken);

                eventsToPublish.AddRange(events);
                aggregate.MarkEventsAsCommitted();
            }
        }

        // Publish domain events for projections
        foreach (var domainEvent in eventsToPublish)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        _aggregates.Clear();
        return eventsToPublish.Count;
    }
}
