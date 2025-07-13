using MediatR;
using StudentCardAssignment.Application.Common.Interfaces;
using StudentCardAssignment.Application.Students.Queries.Common;
using StudentCardAssignment.Domain.Students.Events;

namespace StudentCardAssignment.Application.EventHandlers;

public class StudentCreatedDomainEventHandler(IApplicationDbContext context) : INotificationHandler<StudentCreatedDomainEvent>
{
    private readonly IApplicationDbContext _context = context;

    public async Task Handle(StudentCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Create new student read model
        var studentReadModel = new StudentReadModel
        {
            Id = Guid.NewGuid(),
            StudentId = notification.StudentId.Value,
            FirstName = notification.FirstName,
            LastName = notification.LastName,
            FullName = $"{notification.FirstName} {notification.LastName}",
            Email = notification.Email.Value,
            StudentNumber = notification.StudentNumber.Value,
            Status = "Active", // Default status for new students
            CreatedAt = notification.OccurredOn,
            UpdatedAt = notification.OccurredOn,

            // Assignment fields (initially empty)
            HasAssignedCard = false,
            AssignedCardId = null,
            AssignedCardNumber = null,
            AssignedAt = null
        };

        _context.StudentReadModels.Add(studentReadModel);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
