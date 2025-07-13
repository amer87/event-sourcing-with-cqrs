using StudentCardAssignment.Application.Common;
using StudentCardAssignment.Application.Students.Queries.Common;

namespace StudentCardAssignment.Application.Students.Queries.GetAllStudents;

public record GetAllStudentsQuery : IQuery<IEnumerable<StudentReadModel>>;
