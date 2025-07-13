using StudentCardAssignment.Application.Common;

namespace StudentCardAssignment.Application.Students.Commands.CreateStudent;

public record CreateStudentCommand(
    string FirstName,
    string LastName,
    string Email,
    string StudentNumber
) : ICommand<CreateStudentResult>;
