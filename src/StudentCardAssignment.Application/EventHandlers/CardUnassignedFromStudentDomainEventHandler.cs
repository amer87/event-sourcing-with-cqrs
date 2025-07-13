using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentCardAssignment.Application.Common.Interfaces;
using StudentCardAssignment.Domain.Cards.Events;

namespace StudentCardAssignment.Application.EventHandlers;

public class CardUnassignedFromStudentDomainEventHandler : INotificationHandler<CardUnassignedFromStudentDomainEvent>
{
    private readonly IApplicationDbContext _context;

    public CardUnassignedFromStudentDomainEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CardUnassignedFromStudentDomainEvent notification, CancellationToken cancellationToken)
    {
        // Update card read model
        var cardReadModel = await _context.CardReadModels
            .FirstOrDefaultAsync(c => c.CardId == notification.CardId.Value, cancellationToken);

        // Get student information
        var studentReadModel = await _context.StudentReadModels
            .FirstOrDefaultAsync(s => s.StudentId == notification.StudentId.Value, cancellationToken);

        if (cardReadModel != null)
        {
            // Clear card assignment
            cardReadModel.IsAssigned = false;
            cardReadModel.AssignedStudentId = null;
            cardReadModel.AssignedStudentName = null;
            cardReadModel.AssignedStudentEmail = null;
            cardReadModel.AssignedAt = null;
            cardReadModel.UpdatedAt = notification.OccurredOn;
        }

        if (studentReadModel != null)
        {
            // Clear student assignment
            studentReadModel.HasAssignedCard = false;
            studentReadModel.AssignedCardId = null;
            studentReadModel.AssignedCardNumber = null;
            studentReadModel.AssignedAt = null;
            studentReadModel.UpdatedAt = notification.OccurredOn;
        }

        if (cardReadModel != null || studentReadModel != null)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
