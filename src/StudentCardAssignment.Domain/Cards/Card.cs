using StudentCardAssignment.Domain.Common;
using StudentCardAssignment.Domain.Cards.ValueObjects;
using StudentCardAssignment.Domain.Cards.Events;
using StudentCardAssignment.Domain.Cards.Enums;
using StudentCardAssignment.Domain.Students.ValueObjects;

namespace StudentCardAssignment.Domain.Cards;

public class Card : AggregateRoot
{
    public CardId CardId { get; private set; }
    public CardNumber CardNumber { get; private set; }
    public CardType CardType { get; private set; }
    public CardStatus Status { get; private set; }
    public DateTime IssuedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private CardAssignment? _currentAssignment;
    public CardAssignment? CurrentAssignment => _currentAssignment;

    public Card() : base()
    {
        // For EF Core and event sourcing
        CardId = null!;
        CardNumber = null!;
    }

    private Card(
        CardId cardId,
        CardNumber cardNumber,
        CardType cardType,
        DateTime expiresAt) : base(cardId.Value)
    {
        CardId = cardId;
        CardNumber = cardNumber;
        CardType = cardType;
        Status = CardStatus.Active;
        IssuedAt = DateTime.UtcNow;
        ExpiresAt = expiresAt;
    }

    public static Card Create(CardType cardType, DateTime expiresAt)
    {
        var cardId = CardId.CreateUnique();
        var cardNumber = CardNumber.Generate();

        var card = new Card();
        card.ApplyEvent(new CardCreatedDomainEvent(cardId, cardNumber, cardType, expiresAt), isNew: true);

        return card;
    }

    public void AssignToStudent(StudentId studentId)
    {
        if (IsAssigned)
            throw new InvalidOperationException("Card is already assigned to a student");

        if (Status != CardStatus.Active)
            throw new InvalidOperationException("Cannot assign an inactive card");

        if (IsExpired)
            throw new InvalidOperationException("Cannot assign an expired card");

        ApplyEvent(new CardAssignedToStudentDomainEvent(CardId, studentId, DateTime.UtcNow), isNew: true);
    }

    public void UnassignFromStudent()
    {
        if (!IsAssigned)
            throw new InvalidOperationException("Card is not currently assigned");

        var studentId = _currentAssignment!.StudentId;
        var unassignedAt = DateTime.UtcNow;

        ApplyEvent(new CardUnassignedFromStudentDomainEvent(CardId, studentId, unassignedAt), isNew: true);
    }

    public void ChangeStatus(CardStatus newStatus)
    {
        if (Status == newStatus)
            return;

        var previousStatus = Status;

        // If card is being deactivated and is assigned, unassign it
        if (newStatus != CardStatus.Active && IsAssigned)
        {
            UnassignFromStudent();
        }

        // Directly update status without event
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ReportLost()
    {
        ChangeStatus(CardStatus.Lost);
    }

    public void ReportStolen()
    {
        ChangeStatus(CardStatus.Stolen);
    }

    public void ReportDamaged()
    {
        ChangeStatus(CardStatus.Damaged);
    }

    public bool IsAssigned => _currentAssignment?.IsActive == true;
    public bool IsExpired => DateTime.UtcNow > ExpiresAt;
    public bool IsActive => Status == CardStatus.Active && !IsExpired;
    public StudentId? AssignedStudentId => _currentAssignment?.IsActive == true ? _currentAssignment.StudentId : null;

    protected override void ApplyEvent(IDomainEvent @event)
    {
        switch (@event)
        {
            case CardCreatedDomainEvent cardCreated:
                Apply(cardCreated);
                break;
            case CardAssignedToStudentDomainEvent cardAssigned:
                Apply(cardAssigned);
                break;
            case CardUnassignedFromStudentDomainEvent cardUnassigned:
                Apply(cardUnassigned);
                break;
            default:
                throw new ArgumentException($"Unknown event type: {@event.GetType().Name}");
        }
    }

    private void Apply(CardCreatedDomainEvent @event)
    {
        Id = @event.CardId.Value;
        CardId = @event.CardId;
        CardNumber = @event.CardNumber;
        CardType = @event.CardType;
        Status = CardStatus.Active;
        IssuedAt = @event.OccurredOn;
        ExpiresAt = @event.ExpiresAt;
    }

    private void Apply(CardAssignedToStudentDomainEvent @event)
    {
        _currentAssignment = CardAssignment.Create(@event.StudentId);
        UpdatedAt = @event.OccurredOn;
    }

    private void Apply(CardUnassignedFromStudentDomainEvent @event)
    {
        _currentAssignment = _currentAssignment?.Unassign();
        UpdatedAt = @event.OccurredOn;
    }
}
