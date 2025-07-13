using StudentCardAssignment.Domain.Common;
using StudentCardAssignment.Domain.Cards.ValueObjects;
using StudentCardAssignment.Domain.Students.ValueObjects;

namespace StudentCardAssignment.Domain.Cards.Events;

public class CardUnassignedFromStudentDomainEvent(CardId cardId, StudentId studentId, DateTime unassignedAt) : DomainEvent
{
    public CardId CardId { get; } = cardId;
    public StudentId StudentId { get; } = studentId;
    public DateTime UnassignedAt { get; } = unassignedAt;
}
