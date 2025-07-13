namespace StudentCardAssignment.Application.Cards.Commands.UnassignCard;

public record UnassignCardResult(
    Guid CardId,
    string CardNumber,
    string Status,
    DateTime UnassignedAt
);
