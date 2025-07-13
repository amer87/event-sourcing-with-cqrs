using StudentCardAssignment.Domain.Common;
using StudentCardAssignment.Domain.Students.ValueObjects;
using System.Text.Json.Serialization;

namespace StudentCardAssignment.Domain.Cards.ValueObjects;

public class CardAssignment : ValueObject
{
    public StudentId StudentId { get; }
    public DateTime AssignedAt { get; }
    public DateTime? UnassignedAt { get; }
    public bool IsActive => UnassignedAt == null;

    [JsonConstructor]
    private CardAssignment(StudentId studentId, DateTime assignedAt, DateTime? unassignedAt = null)
    {
        StudentId = studentId;
        AssignedAt = assignedAt;
        UnassignedAt = unassignedAt;
    }

    public static CardAssignment Create(StudentId studentId)
    {
        return new CardAssignment(studentId, DateTime.UtcNow);
    }

    public CardAssignment Unassign()
    {
        if (UnassignedAt.HasValue)
            throw new InvalidOperationException("Card is already unassigned");

        return new CardAssignment(StudentId, AssignedAt, DateTime.UtcNow);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StudentId;
        yield return AssignedAt;
        yield return UnassignedAt ?? (object)"null";
    }
}
