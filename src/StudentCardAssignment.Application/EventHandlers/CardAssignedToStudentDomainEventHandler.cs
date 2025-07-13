using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentCardAssignment.Application.Common.Interfaces;
using StudentCardAssignment.Domain.Cards.Events;

namespace StudentCardAssignment.Application.EventHandlers;

public class CardAssignedToStudentDomainEventHandler(IApplicationDbContext context) : INotificationHandler<CardAssignedToStudentDomainEvent>
{
    private readonly IApplicationDbContext _context = context;

    public async Task Handle(CardAssignedToStudentDomainEvent notification, CancellationToken cancellationToken)
    {
        // Update card read model
        var cardReadModel = await _context.CardReadModels
            .FirstOrDefaultAsync(c => c.CardId == notification.CardId.Value, cancellationToken);

        // Get student information
        var studentReadModel = await _context.StudentReadModels
            .FirstOrDefaultAsync(s => s.StudentId == notification.StudentId.Value, cancellationToken);

        if (cardReadModel != null && studentReadModel != null)
        {
            // Update card assignment
            cardReadModel.IsAssigned = true;
            cardReadModel.AssignedStudentId = notification.StudentId.Value;
            cardReadModel.AssignedStudentName = studentReadModel.FullName;
            cardReadModel.AssignedStudentEmail = studentReadModel.Email;
            cardReadModel.AssignedAt = notification.AssignedAt;
            cardReadModel.UpdatedAt = notification.OccurredOn;

            // Update student assignment
            studentReadModel.HasAssignedCard = true;
            studentReadModel.AssignedCardId = notification.CardId.Value;
            studentReadModel.AssignedCardNumber = cardReadModel.MaskedCardNumber;
            studentReadModel.AssignedAt = notification.AssignedAt;
            studentReadModel.UpdatedAt = notification.OccurredOn;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
