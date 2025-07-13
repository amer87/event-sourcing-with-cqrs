using StudentCardAssignment.Application.Common;

namespace StudentCardAssignment.Application.Students.Queries.GetStudent;

public record GetStudentQuery(Guid StudentId) : IQuery<GetStudentResult?>;
