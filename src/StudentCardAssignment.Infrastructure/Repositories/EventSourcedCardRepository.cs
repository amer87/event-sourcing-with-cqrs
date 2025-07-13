using Microsoft.EntityFrameworkCore;
using StudentCardAssignment.Application.Common.Interfaces;
using StudentCardAssignment.Domain.Cards;
using StudentCardAssignment.Domain.Cards.ValueObjects;
using StudentCardAssignment.Domain.Students.ValueObjects;
using StudentCardAssignment.Infrastructure.EventStore;
using MediatR;

namespace StudentCardAssignment.Infrastructure.Repositories;

public class EventSourcedCardRepository(
    IEventStore eventStore,
    IApplicationDbContext context,
    IMediator mediator) : IEventSourcedCardRepository
{
    private readonly IEventStore _eventStore = eventStore;
    private readonly IApplicationDbContext _context = context;
    private readonly IMediator _mediator = mediator;

    public async Task<Card?> GetByIdAsync(CardId id, CancellationToken cancellationToken = default)
    {
        var events = await _eventStore.GetEventsAsync(id.Value, cancellationToken);
        if (!events.Any())
            return null;

        var card = new Card();
        card.LoadFromHistory(events);
        return card;
    }

    public async Task<Card?> GetByCardNumberAsync(CardNumber cardNumber, CancellationToken cancellationToken = default)
    {
        // For queries like this, we need to use read models or maintain lookup tables
        // For now, we'll use the read model to get the CardId, then load from events
        var cardReadModel = await _context.CardReadModels
            .FirstOrDefaultAsync(c => c.CardNumber == cardNumber.Value, cancellationToken);

        if (cardReadModel == null)
            return null;

        return await GetByIdAsync(CardId.Create(cardReadModel.CardId), cancellationToken);
    }

    public async Task<Card?> GetByAssignedStudentIdAsync(StudentId studentId, CancellationToken cancellationToken = default)
    {
        // Use read model to find the card assigned to this student
        var cardReadModel = await _context.CardReadModels
            .FirstOrDefaultAsync(c => c.AssignedStudentId == studentId.Value, cancellationToken);

        if (cardReadModel == null)
            return null;

        return await GetByIdAsync(CardId.Create(cardReadModel.CardId), cancellationToken);
    }

    public async Task SaveAsync(Card card, CancellationToken cancellationToken = default)
    {
        var events = card.DomainEvents;
        if (events.Any())
        {
            await _eventStore.SaveEventsAsync(
                card.Id,
                nameof(Card),
                events,
                card.Version,
                cancellationToken);

            // Publish events to update projections
            foreach (var domainEvent in events)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }

            card.MarkEventsAsCommitted();
        }
    }
}
