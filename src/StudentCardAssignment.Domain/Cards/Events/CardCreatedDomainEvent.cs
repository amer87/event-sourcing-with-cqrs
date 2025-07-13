using StudentCardAssignment.Domain.Common;
using StudentCardAssignment.Domain.Cards.ValueObjects;
using StudentCardAssignment.Domain.Cards.Enums;

namespace StudentCardAssignment.Domain.Cards.Events;

public class CardCreatedDomainEvent(CardId cardId, CardNumber cardNumber, CardType cardType, DateTime expiresAt) : DomainEvent
{
    public CardId CardId { get; } = cardId;
    public CardNumber CardNumber { get; } = cardNumber;
    public CardType CardType { get; } = cardType;
    public DateTime ExpiresAt { get; } = expiresAt;
}
