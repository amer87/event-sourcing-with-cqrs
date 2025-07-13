using StudentCardAssignment.Application.Common;

namespace StudentCardAssignment.Application.Cards.Commands.AssignCard;

public record AssignCardToStudentCommand(
    Guid CardId,
    Guid StudentId
) : ICommand<AssignCardToStudentResult>;
