using StudentCardAssignment.Domain.Cards.Enums;

namespace StudentCardAssignment.Application.Cards.Commands.CreateCard;

public record CreateCardResult(
    Guid CardId,
    string CardNumber,
    CardType CardType,
    DateTime IssuedAt,
    DateTime ExpiresAt
);
