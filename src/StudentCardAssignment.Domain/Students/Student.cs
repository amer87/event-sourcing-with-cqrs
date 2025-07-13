using StudentCardAssignment.Domain.Common;
using StudentCardAssignment.Domain.Students.ValueObjects;
using StudentCardAssignment.Domain.Students.Events;
using StudentCardAssignment.Domain.Students.Enums;

namespace StudentCardAssignment.Domain.Students;

public partial class Student : AggregateRoot
{
    public StudentId StudentId { get; private set; }
    public PersonName FirstName { get; private set; }
    public PersonName LastName { get; private set; }
    public Email Email { get; private set; }
    public StudentNumber StudentNumber { get; private set; }
    public StudentStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public Student() : base()
    {
        // EF Core constructor and event sourcing - properties will be set by EF
        StudentId = null!;
        FirstName = null!;
        LastName = null!;
        Email = null!;
        StudentNumber = null!;
    }

    private Student(
        StudentId studentId,
        PersonName firstName,
        PersonName lastName,
        Email email,
        StudentNumber studentNumber) : base(studentId.Value)
    {
        StudentId = studentId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        StudentNumber = studentNumber;
        Status = StudentStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

    public static Student Create(
        string firstName,
        string lastName,
        Email email,
        StudentNumber studentNumber)
    {
        var studentId = StudentId.CreateUnique();
        var student = new Student();

        var createdEvent = new StudentCreatedDomainEvent(
            studentId, firstName, lastName, email, studentNumber);

        student.ApplyEvent(createdEvent, isNew: true);

        return student;
    }

    public void UpdateProfile(string firstName, string lastName, Email email)
    {
        var firstNameVO = PersonName.Create(firstName);
        var lastNameVO = PersonName.Create(lastName);

        // Create and apply profile updated event
        var profileUpdatedEvent = new StudentProfileUpdatedDomainEvent(
            StudentId, firstNameVO.Value, lastNameVO.Value, email);

        ApplyEvent(profileUpdatedEvent, isNew: true);
    }

    public void ChangeStatus(StudentStatus newStatus)
    {
        if (Status == newStatus)
            return;

        var previousStatus = Status;
        var statusChangedEvent = new StudentStatusChangedDomainEvent(StudentId, previousStatus, newStatus);
        ApplyEvent(statusChangedEvent, isNew: true);
    }

    public string GetFullName() => $"{FirstName} {LastName}";

    public bool IsActive => Status == StudentStatus.Active;

    protected override void ApplyEvent(IDomainEvent @event)
    {
        switch (@event)
        {
            case StudentCreatedDomainEvent studentCreated:
                Apply(studentCreated);
                break;
            case StudentStatusChangedDomainEvent statusChanged:
                Apply(statusChanged);
                break;
            case StudentProfileUpdatedDomainEvent profileUpdated:
                Apply(profileUpdated);
                break;
            default:
                throw new ArgumentException($"Unknown event type: {@event.GetType().Name}");
        }
    }

    private void Apply(StudentCreatedDomainEvent @event)
    {
        Id = @event.StudentId.Value;
        StudentId = @event.StudentId;
        FirstName = PersonName.Create(@event.FirstName);
        LastName = PersonName.Create(@event.LastName);
        Email = @event.Email;
        StudentNumber = @event.StudentNumber;
        Status = StudentStatus.Active;
        CreatedAt = @event.OccurredOn;
    }

    private void Apply(StudentStatusChangedDomainEvent @event)
    {
        Status = @event.NewStatus;
        UpdatedAt = @event.OccurredOn;
    }

    private void Apply(StudentProfileUpdatedDomainEvent @event)
    {
        FirstName = PersonName.Create(@event.FirstName);
        LastName = PersonName.Create(@event.LastName);
        Email = @event.Email;
        UpdatedAt = @event.OccurredOn;
    }
}
