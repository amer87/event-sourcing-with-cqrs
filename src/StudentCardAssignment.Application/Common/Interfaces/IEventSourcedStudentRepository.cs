using StudentCardAssignment.Domain.Students;
using StudentCardAssignment.Domain.Students.ValueObjects;

namespace StudentCardAssignment.Application.Common.Interfaces;

public interface IEventSourcedStudentRepository
{
    Task<Student?> GetByIdAsync(StudentId id, CancellationToken cancellationToken = default);
    Task<Student?> GetByStudentNumberAsync(StudentNumber studentNumber, CancellationToken cancellationToken = default);
    Task<Student?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
    Task SaveAsync(Student student, CancellationToken cancellationToken = default);
}
