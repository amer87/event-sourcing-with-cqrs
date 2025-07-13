using StudentCardAssignment.Application.Common;
using StudentCardAssignment.Application.Common.Interfaces;
using StudentCardAssignment.Domain.Cards;

namespace StudentCardAssignment.Application.Cards.Commands.CreateCard;

internal class CreateCardCommandHandler(
    IEventSourcedCardRepository cardRepository) : ICommandHandler<CreateCardCommand, CreateCardResult>
{
    private readonly IEventSourcedCardRepository _cardRepository = cardRepository;

    public async Task<CreateCardResult> Handle(CreateCardCommand request, CancellationToken cancellationToken)
    {
        if (request.ExpiresAt <= DateTime.UtcNow)
        {
            throw new ArgumentException("Expiration date must be in the future");
        }

        // Create card - this generates domain events
        var card = Card.Create(request.CardType, request.ExpiresAt);

        // Save to event store - this persists events and publishes them automatically
        await _cardRepository.SaveAsync(card, cancellationToken);

        return new CreateCardResult(
            card.CardId.Value,
            card.CardNumber.Value,
            card.CardType,
            card.IssuedAt,
            card.ExpiresAt);
    }
}
