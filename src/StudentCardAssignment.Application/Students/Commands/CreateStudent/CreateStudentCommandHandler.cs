using StudentCardAssignment.Application.Common;
using StudentCardAssignment.Application.Common.Interfaces;
using StudentCardAssignment.Domain.Students;
using StudentCardAssignment.Domain.Students.ValueObjects;

namespace StudentCardAssignment.Application.Students.Commands.CreateStudent;

public class CreateStudentCommandHandler(
    IEventSourcedStudentRepository studentRepository) : ICommandHandler<CreateStudentCommand, CreateStudentResult>
{
    private readonly IEventSourcedStudentRepository _studentRepository = studentRepository;

    public async Task<CreateStudentResult> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        // Check if student with email already exists
        var existingStudentByEmail = await _studentRepository.GetByEmailAsync(
            Email.Create(request.Email), cancellationToken);

        if (existingStudentByEmail is not null)
        {
            throw new InvalidOperationException($"Student with email '{request.Email}' already exists");
        }

        // Check if student with student number already exists
        var existingStudentByNumber = await _studentRepository.GetByStudentNumberAsync(
            StudentNumber.Create(request.StudentNumber), cancellationToken);

        if (existingStudentByNumber is not null)
        {
            throw new InvalidOperationException($"Student with number '{request.StudentNumber}' already exists");
        }

        // Create student - this generates domain events
        var student = Student.Create(
            request.FirstName,
            request.LastName,
            Email.Create(request.Email),
            StudentNumber.Create(request.StudentNumber));

        // Save to event store - this persists events and publishes them automatically
        await _studentRepository.SaveAsync(student, cancellationToken);

        return new CreateStudentResult(
            student.StudentId.Value,
            student.FirstName,
            student.LastName,
            student.Email.Value,
            student.StudentNumber.Value);
    }
}
