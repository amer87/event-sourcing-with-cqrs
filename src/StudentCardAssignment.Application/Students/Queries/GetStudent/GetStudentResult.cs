using StudentCardAssignment.Domain.Students.Enums;

namespace StudentCardAssignment.Application.Students.Queries.GetStudent;

public record GetStudentResult(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string StudentNumber,
    StudentStatus Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
