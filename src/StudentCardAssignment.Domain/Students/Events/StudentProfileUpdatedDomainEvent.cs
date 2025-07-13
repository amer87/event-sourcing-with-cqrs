using StudentCardAssignment.Domain.Common;
using StudentCardAssignment.Domain.Students.ValueObjects;

namespace StudentCardAssignment.Domain.Students.Events;

public class StudentProfileUpdatedDomainEvent : DomainEvent
{
    public StudentId StudentId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public Email Email { get; }

    public StudentProfileUpdatedDomainEvent(StudentId studentId, string firstName, string lastName, Email email)
    {
        StudentId = studentId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}
