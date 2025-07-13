using StudentCardAssignment.Application.Students.Queries.Common;
using StudentCardAssignment.Application.Cards.Queries.Common;

namespace StudentCardAssignment.Application.Common.Interfaces;

// Read-only repository interfaces for projections
public interface IStudentReadModelRepository
{
    Task<StudentReadModel?> GetByIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<StudentReadModel?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<StudentReadModel?> GetByStudentNumberAsync(string studentNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<StudentReadModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<StudentReadModel>> GetActiveStudentsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<StudentReadModel>> SearchStudentsAsync(string searchTerm, CancellationToken cancellationToken = default);
}

public interface ICardReadModelRepository
{
    Task<CardReadModel?> GetByIdAsync(Guid cardId, CancellationToken cancellationToken = default);
    Task<CardReadModel?> GetByCardNumberAsync(string cardNumber, CancellationToken cancellationToken = default);
    Task<CardReadModel?> GetByAssignedStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CardReadModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CardReadModel>> GetAvailableCardsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CardReadModel>> GetAssignedCardsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CardReadModel>> GetExpiringCardsAsync(int daysFromNow, CancellationToken cancellationToken = default);
}
