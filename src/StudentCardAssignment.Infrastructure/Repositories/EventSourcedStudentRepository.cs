using Microsoft.EntityFrameworkCore;
using StudentCardAssignment.Application.Common.Interfaces;
using StudentCardAssignment.Domain.Students;
using StudentCardAssignment.Domain.Students.ValueObjects;
using StudentCardAssignment.Infrastructure.EventStore;
using MediatR;

namespace StudentCardAssignment.Infrastructure.Repositories;

/// <summary>
/// Event-sourced repository that reconstructs Student aggregates from event history
/// This is an ALTERNATIVE to the EF Core-based StudentRepository
/// </summary>
public class EventSourcedStudentRepository(IEventStore eventStore, IApplicationDbContext context, IMediator mediator) : IEventSourcedStudentRepository
{
    private readonly IEventStore _eventStore = eventStore;
    private readonly IApplicationDbContext _context = context;
    private readonly IMediator _mediator = mediator;

    public async Task<Student?> GetByIdAsync(StudentId id, CancellationToken cancellationToken = default)
    {
        var events = await _eventStore.GetEventsAsync(id.Value, cancellationToken);
        if (!events.Any())
            return null;

        var student = new Student();
        student.LoadFromHistory(events);
        return student;
    }

    public async Task<Student?> GetByStudentNumberAsync(StudentNumber studentNumber, CancellationToken cancellationToken = default)
    {
        // Use read model to find the student by student number
        var studentReadModel = await _context.StudentReadModels
            .FirstOrDefaultAsync(s => s.StudentNumber == studentNumber.Value, cancellationToken);

        if (studentReadModel == null)
            return null;

        return await GetByIdAsync(StudentId.Create(studentReadModel.StudentId), cancellationToken);
    }

    public async Task<Student?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        // Use read model to find the student by email
        var studentReadModel = await _context.StudentReadModels
            .FirstOrDefaultAsync(s => s.Email == email.Value, cancellationToken);

        if (studentReadModel == null)
            return null;

        return await GetByIdAsync(StudentId.Create(studentReadModel.StudentId), cancellationToken);
    }

    public async Task SaveAsync(Student student, CancellationToken cancellationToken = default)
    {
        var events = student.DomainEvents;
        if (events.Any())
        {
            await _eventStore.SaveEventsAsync(
                student.Id,
                nameof(Student),
                events,
                student.Version,
                cancellationToken);

            // Publish events to update projections
            foreach (var domainEvent in events)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }

            student.MarkEventsAsCommitted();
        }
    }
}
