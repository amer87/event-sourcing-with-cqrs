using StudentCardAssignment.Application.Common;
using StudentCardAssignment.Domain.Cards.Enums;

namespace StudentCardAssignment.Application.Cards.Commands.CreateCard;

public record CreateCardCommand(
    CardType CardType,
    DateTime ExpiresAt
) : ICommand<CreateCardResult>;
