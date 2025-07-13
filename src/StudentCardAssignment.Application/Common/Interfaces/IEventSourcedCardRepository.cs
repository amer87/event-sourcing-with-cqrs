using StudentCardAssignment.Domain.Cards;
using StudentCardAssignment.Domain.Cards.ValueObjects;
using StudentCardAssignment.Domain.Students.ValueObjects;

namespace StudentCardAssignment.Application.Common.Interfaces;

public interface IEventSourcedCardRepository
{
    Task<Card?> GetByIdAsync(CardId id, CancellationToken cancellationToken = default);
    Task<Card?> GetByCardNumberAsync(CardNumber cardNumber, CancellationToken cancellationToken = default);
    Task<Card?> GetByAssignedStudentIdAsync(StudentId studentId, CancellationToken cancellationToken = default);
    Task SaveAsync(Card card, CancellationToken cancellationToken = default);
}
