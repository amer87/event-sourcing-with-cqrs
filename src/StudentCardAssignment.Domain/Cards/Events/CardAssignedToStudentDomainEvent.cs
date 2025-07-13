using StudentCardAssignment.Domain.Common;
using StudentCardAssignment.Domain.Cards.ValueObjects;
using StudentCardAssignment.Domain.Students.ValueObjects;

namespace StudentCardAssignment.Domain.Cards.Events;

public class CardAssignedToStudentDomainEvent(CardId cardId, StudentId studentId, DateTime assignedAt) : DomainEvent
{
    public CardId CardId { get; } = cardId;
    public StudentId StudentId { get; } = studentId;
    public DateTime AssignedAt { get; } = assignedAt;
}
