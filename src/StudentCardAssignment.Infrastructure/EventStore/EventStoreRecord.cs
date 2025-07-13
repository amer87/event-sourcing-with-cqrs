namespace StudentCardAssignment.Infrastructure.EventStore;

public class EventStoreRecord
{
    public Guid Id { get; set; }
    public Guid AggregateId { get; set; }
    public string AggregateType { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string EventData { get; set; } = string.Empty;
    public string? Metadata { get; set; }
    public DateTime OccurredOn { get; set; }
    public long Version { get; set; }
    public DateTime CreatedAt { get; set; }
}
