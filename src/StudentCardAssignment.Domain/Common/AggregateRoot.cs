namespace StudentCardAssignment.Domain.Common;

public abstract class AggregateRoot : Entity
{
    public long Version { get; protected set; } = -1;

    protected AggregateRoot(Guid id) : base(id)
    {
    }

    protected AggregateRoot() : base()
    {
        // For EF Core
    }

    public void LoadFromHistory(IEnumerable<IDomainEvent> events)
    {
        foreach (var @event in events)
        {
            ApplyEvent(@event, false);
            Version++;
        }
    }

    protected void ApplyEvent(IDomainEvent @event, bool isNew = true)
    {
        this.ApplyEvent(@event);
        if (isNew)
        {
            Version++;
            RaiseDomainEvent(@event);
        }
    }

    protected abstract void ApplyEvent(IDomainEvent @event);

    public void MarkEventsAsCommitted()
    {
        ClearDomainEvents();
    }
}
