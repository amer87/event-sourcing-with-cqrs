namespace StudentCardAssignment.Application.Students.Queries.Common;

// Read Model for Student queries - optimized for reading
public class StudentReadModel
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string StudentNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Additional read-optimized fields
    public bool IsActive { get; set; }
    public string DisplayName { get; set; } = string.Empty;

    // Card assignment properties
    public bool HasAssignedCard { get; set; }
    public Guid? AssignedCardId { get; set; }
    public string? AssignedCardNumber { get; set; }
    public DateTime? AssignedAt { get; set; }
}
