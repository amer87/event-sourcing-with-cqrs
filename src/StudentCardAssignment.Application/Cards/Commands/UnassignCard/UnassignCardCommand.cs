using StudentCardAssignment.Application.Common;

namespace StudentCardAssignment.Application.Cards.Commands.UnassignCard;

public record UnassignCardCommand(
    Guid CardId
) : ICommand<UnassignCardResult>;
