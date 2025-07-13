using Microsoft.AspNetCore.Mvc;
using StudentCardAssignment.Infrastructure.EventStore;

namespace StudentCardAssignment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(IEventStore eventStore) : ControllerBase
{
    private readonly IEventStore _eventStore = eventStore;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventStoreRecord>>> GetAllEvents(CancellationToken cancellationToken)
    {
        var events = await _eventStore.GetAllEventsAsync(cancellationToken);
        return Ok(events);
    }

    [HttpGet("aggregate/{aggregateId:guid}")]
    public async Task<ActionResult<IEnumerable<EventStoreRecord>>> GetEventsByAggregate(
        Guid aggregateId,
        CancellationToken cancellationToken)
    {
        var events = await _eventStore.GetEventsAsync(aggregateId, cancellationToken);
        return Ok(events);
    }
}
