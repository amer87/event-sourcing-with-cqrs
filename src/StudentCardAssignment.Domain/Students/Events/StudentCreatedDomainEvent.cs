using StudentCardAssignment.Domain.Common;
using StudentCardAssignment.Domain.Students.ValueObjects;

namespace StudentCardAssignment.Domain.Students.Events;

public class StudentCreatedDomainEvent : DomainEvent
{
    public StudentId StudentId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public Email Email { get; }
    public StudentNumber StudentNumber { get; }

    public StudentCreatedDomainEvent(StudentId studentId, string firstName, string lastName, Email email, StudentNumber studentNumber)
    {
        StudentId = studentId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        StudentNumber = studentNumber;
    }
}
