using Microsoft.EntityFrameworkCore;
using StudentCardAssignment.Application.Common.Interfaces;
using StudentCardAssignment.Application.Students.Queries.Common;
using StudentCardAssignment.Infrastructure.Persistence;

namespace StudentCardAssignment.Infrastructure.Repositories;

public class StudentReadModelRepository : IStudentReadModelRepository
{
    private readonly ApplicationDbContext _context;

    public StudentReadModelRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentReadModel?> GetByIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await _context.StudentReadModels
            .FirstOrDefaultAsync(s => s.StudentId == studentId, cancellationToken);
    }

    public async Task<StudentReadModel?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.StudentReadModels
            .FirstOrDefaultAsync(s => s.Email == email, cancellationToken);
    }

    public async Task<StudentReadModel?> GetByStudentNumberAsync(string studentNumber, CancellationToken cancellationToken = default)
    {
        return await _context.StudentReadModels
            .FirstOrDefaultAsync(s => s.StudentNumber == studentNumber, cancellationToken);
    }

    public async Task<IEnumerable<StudentReadModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.StudentReadModels
            .OrderBy(s => s.LastName)
            .ThenBy(s => s.FirstName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<StudentReadModel>> GetActiveStudentsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.StudentReadModels
            .Where(s => s.Status == "Active")
            .OrderBy(s => s.LastName)
            .ThenBy(s => s.FirstName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<StudentReadModel>> SearchStudentsAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var normalizedSearchTerm = searchTerm.ToLower();

        return await _context.StudentReadModels
            .Where(s => s.FirstName.ToLower().Contains(normalizedSearchTerm) ||
                       s.LastName.ToLower().Contains(normalizedSearchTerm) ||
                       s.FullName.ToLower().Contains(normalizedSearchTerm) ||
                       s.Email.ToLower().Contains(normalizedSearchTerm) ||
                       s.StudentNumber.ToLower().Contains(normalizedSearchTerm))
            .OrderBy(s => s.LastName)
            .ThenBy(s => s.FirstName)
            .ToListAsync(cancellationToken);
    }
}
