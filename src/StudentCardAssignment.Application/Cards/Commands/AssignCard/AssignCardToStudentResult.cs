namespace StudentCardAssignment.Application.Cards.Commands.AssignCard;

public record AssignCardToStudentResult(
    Guid CardId,
    Guid StudentId,
    DateTime AssignedAt
);
