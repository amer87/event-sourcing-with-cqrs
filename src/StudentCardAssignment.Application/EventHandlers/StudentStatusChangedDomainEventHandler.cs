using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentCardAssignment.Application.Common.Interfaces;
using StudentCardAssignment.Domain.Students.Events;

namespace StudentCardAssignment.Application.EventHandlers;

public class StudentStatusChangedDomainEventHandler : INotificationHandler<StudentStatusChangedDomainEvent>
{
    private readonly IApplicationDbContext _context;

    public StudentStatusChangedDomainEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(StudentStatusChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Update student read model
        var studentReadModel = await _context.StudentReadModels
            .FirstOrDefaultAsync(s => s.StudentId == notification.StudentId.Value, cancellationToken);

        if (studentReadModel != null)
        {
            studentReadModel.Status = notification.NewStatus.ToString();
            studentReadModel.UpdatedAt = notification.OccurredOn;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
