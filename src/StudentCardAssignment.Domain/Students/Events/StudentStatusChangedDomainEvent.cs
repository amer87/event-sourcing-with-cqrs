using StudentCardAssignment.Domain.Common;
using StudentCardAssignment.Domain.Students.ValueObjects;
using StudentCardAssignment.Domain.Students.Enums;

namespace StudentCardAssignment.Domain.Students.Events;

public class StudentStatusChangedDomainEvent : DomainEvent
{
    public StudentId StudentId { get; }
    public StudentStatus PreviousStatus { get; }
    public StudentStatus NewStatus { get; }

    public StudentStatusChangedDomainEvent(StudentId studentId, StudentStatus previousStatus, StudentStatus newStatus)
    {
        StudentId = studentId;
        PreviousStatus = previousStatus;
        NewStatus = newStatus;
    }
}
