namespace StudentCardAssignment.Application.Students.Commands.CreateStudent;

public record CreateStudentResult(
    Guid StudentId,
    string FirstName,
    string LastName,
    string Email,
    string StudentNumber
);
