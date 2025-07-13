using StudentCardAssignment.Application.Common;
using StudentCardAssignment.Application.Common.Interfaces;
using StudentCardAssignment.Application.Students.Queries.Common;

namespace StudentCardAssignment.Application.Students.Queries.GetAllStudents;

internal class GetAllStudentsQueryHandler : IQueryHandler<GetAllStudentsQuery, IEnumerable<StudentReadModel>>
{
    private readonly IStudentReadModelRepository _studentReadModelRepository;

    public GetAllStudentsQueryHandler(IStudentReadModelRepository studentReadModelRepository)
    {
        _studentReadModelRepository = studentReadModelRepository;
    }

    public async Task<IEnumerable<StudentReadModel>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        return await _studentReadModelRepository.GetAllAsync(cancellationToken);
    }
}
