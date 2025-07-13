using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentCardAssignment.Application.Cards.Queries.Common;
using StudentCardAssignment.Application.Common.Interfaces;
using StudentCardAssignment.Domain.Cards.Events;

namespace StudentCardAssignment.Application.EventHandlers;

public class CardCreatedDomainEventHandler : INotificationHandler<CardCreatedDomainEvent>
{
    private readonly IApplicationDbContext _context;

    public CardCreatedDomainEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CardCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Create masked card number (show only last 4 digits)
        var cardNumber = notification.CardNumber.Value;
        var maskedCardNumber = "****-****-****-" + cardNumber.Substring(cardNumber.Length - 4);

        // Create new card read model
        var cardReadModel = new CardReadModel
        {
            Id = Guid.NewGuid(),
            CardId = notification.CardId.Value,
            CardNumber = cardNumber,
            MaskedCardNumber = maskedCardNumber,
            CardType = notification.CardType.ToString(),
            Status = "Active", // Default status for new cards
            IssuedAt = notification.OccurredOn,
            ExpiresAt = notification.OccurredOn.AddYears(3), // Default 3-year expiry
            UpdatedAt = notification.OccurredOn,

            // Assignment fields (initially empty)
            IsAssigned = false,
            AssignedStudentId = null,
            AssignedStudentName = null,
            AssignedStudentEmail = null,
            AssignedAt = null,

            // Computed fields
            IsActive = true,
            IsExpired = false,
            DaysUntilExpiry = (int)(notification.OccurredOn.AddYears(3) - DateTime.UtcNow).TotalDays
        };

        _context.CardReadModels.Add(cardReadModel);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
