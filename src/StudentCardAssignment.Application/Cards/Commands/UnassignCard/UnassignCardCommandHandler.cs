using StudentCardAssignment.Application.Common;
using StudentCardAssignment.Application.Common.Interfaces;
using StudentCardAssignment.Domain.Cards.ValueObjects;

namespace StudentCardAssignment.Application.Cards.Commands.UnassignCard;

public class UnassignCardCommandHandler(
    IEventSourcedCardRepository cardRepository) : ICommandHandler<UnassignCardCommand, UnassignCardResult>
{
    private readonly IEventSourcedCardRepository _cardRepository = cardRepository;

    public async Task<UnassignCardResult> Handle(UnassignCardCommand request, CancellationToken cancellationToken)
    {
        var card = await _cardRepository.GetByIdAsync(
            CardId.Create(request.CardId), cancellationToken);

        if (card is null)
        {
            throw new InvalidOperationException($"Card with ID '{request.CardId}' not found");
        }

        if (!card.IsAssigned)
        {
            throw new InvalidOperationException("Card is not currently assigned to any student");
        }

        // Unassign the card from the student
        card.UnassignFromStudent();

        // Save the card (this will persist the events and publish them)
        await _cardRepository.SaveAsync(card, cancellationToken);

        return new UnassignCardResult(
            card.CardId.Value,
            card.CardNumber.Value,
            card.Status.ToString(),
            DateTime.UtcNow
        );
    }
}
