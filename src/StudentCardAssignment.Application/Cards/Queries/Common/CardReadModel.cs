namespace StudentCardAssignment.Application.Cards.Queries.Common;

// Read Model for Card queries - optimized for reading
public class CardReadModel
{
    public Guid Id { get; set; }
    public Guid CardId { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public string MaskedCardNumber { get; set; } = string.Empty;
    public string CardType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Assignment information (denormalized for read performance)
    public bool IsAssigned { get; set; }
    public Guid? AssignedStudentId { get; set; }
    public string? AssignedStudentName { get; set; }
    public string? AssignedStudentEmail { get; set; }
    public DateTime? AssignedAt { get; set; }

    // Computed fields
    public bool IsActive { get; set; }
    public bool IsExpired { get; set; }
    public int DaysUntilExpiry { get; set; }
}
