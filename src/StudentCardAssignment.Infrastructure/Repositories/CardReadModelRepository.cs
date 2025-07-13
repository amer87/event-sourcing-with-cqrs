using Microsoft.EntityFrameworkCore;
using StudentCardAssignment.Application.Common.Interfaces;
using StudentCardAssignment.Application.Cards.Queries.Common;
using StudentCardAssignment.Infrastructure.Persistence;

namespace StudentCardAssignment.Infrastructure.Repositories;

public class CardReadModelRepository : ICardReadModelRepository
{
    private readonly ApplicationDbContext _context;

    public CardReadModelRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CardReadModel?> GetByIdAsync(Guid cardId, CancellationToken cancellationToken = default)
    {
        return await _context.CardReadModels
            .FirstOrDefaultAsync(c => c.CardId == cardId, cancellationToken);
    }

    public async Task<CardReadModel?> GetByCardNumberAsync(string cardNumber, CancellationToken cancellationToken = default)
    {
        return await _context.CardReadModels
            .FirstOrDefaultAsync(c => c.CardNumber == cardNumber, cancellationToken);
    }

    public async Task<CardReadModel?> GetByAssignedStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await _context.CardReadModels
            .FirstOrDefaultAsync(c => c.AssignedStudentId == studentId && c.IsAssigned, cancellationToken);
    }

    public async Task<IEnumerable<CardReadModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CardReadModels
            .OrderBy(c => c.CardNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CardReadModel>> GetAvailableCardsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CardReadModels
            .Where(c => !c.IsAssigned && c.IsActive && !c.IsExpired)
            .OrderBy(c => c.CardNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CardReadModel>> GetAssignedCardsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CardReadModels
            .Where(c => c.IsAssigned)
            .OrderBy(c => c.AssignedStudentName)
            .ThenBy(c => c.CardNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CardReadModel>> GetExpiringCardsAsync(int daysFromNow, CancellationToken cancellationToken = default)
    {
        var expiryThreshold = DateTime.UtcNow.AddDays(daysFromNow);

        return await _context.CardReadModels
            .Where(c => c.ExpiresAt <= expiryThreshold && c.IsActive)
            .OrderBy(c => c.ExpiresAt)
            .ThenBy(c => c.CardNumber)
            .ToListAsync(cancellationToken);
    }
}
