using StudentCardAssignment.Application.Common;
using StudentCardAssignment.Application.Common.Interfaces;

namespace StudentCardAssignment.Application.Students.Queries.GetStudent;

internal class GetStudentQueryHandler : IQueryHandler<GetStudentQuery, GetStudentResult?>
{
    private readonly IStudentReadModelRepository _studentReadModelRepository;

    public GetStudentQueryHandler(IStudentReadModelRepository studentReadModelRepository)
    {
        _studentReadModelRepository = studentReadModelRepository;
    }

    public async Task<GetStudentResult?> Handle(GetStudentQuery request, CancellationToken cancellationToken)
    {
        var student = await _studentReadModelRepository.GetByIdAsync(request.StudentId, cancellationToken);

        if (student is null)
            return null;

        return new GetStudentResult(
            student.StudentId,
            student.FirstName,
            student.LastName,
            student.Email,
            student.StudentNumber,
            Enum.Parse<StudentCardAssignment.Domain.Students.Enums.StudentStatus>(student.Status),
            student.CreatedAt,
            student.UpdatedAt);
    }
}
