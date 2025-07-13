using FluentAssertions;
using StudentCardAssignment.Domain.Cards;
using StudentCardAssignment.Domain.Cards.ValueObjects;
using StudentCardAssignment.Domain.Cards.Events;
using StudentCardAssignment.Domain.Cards.Enums;
using StudentCardAssignment.Domain.Students.ValueObjects;
using Xunit;

namespace StudentCardAssignment.Domain.Tests.Cards;

public class CardTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateCard()
    {
        // Arrange
        var cardType = CardType.Student;
        var expiresAt = DateTime.UtcNow.AddYears(1);

        // Act
        var card = Card.Create(cardType, expiresAt);

        // Assert
        card.Should().NotBeNull();
        card.CardType.Should().Be(cardType);
        card.ExpiresAt.Should().Be(expiresAt);
        card.Status.Should().Be(CardStatus.Active);
        card.IsActive.Should().BeTrue();
        card.IsAssigned.Should().BeFalse();
        card.IsExpired.Should().BeFalse();
        card.DomainEvents.Should().HaveCount(1);
        card.DomainEvents.First().Should().BeOfType<CardCreatedDomainEvent>();
    }

    [Fact]
    public void AssignToStudent_WithValidStudent_ShouldAssignCard()
    {
        // Arrange
        var card = CreateValidCard();
        var studentId = StudentId.CreateUnique();

        // Act
        card.AssignToStudent(studentId);

        // Assert
        card.IsAssigned.Should().BeTrue();
        card.AssignedStudentId.Should().Be(studentId);
        card.CurrentAssignment.Should().NotBeNull();
        card.CurrentAssignment!.StudentId.Should().Be(studentId);
        card.CurrentAssignment.IsActive.Should().BeTrue();
        card.DomainEvents.Should().HaveCount(2); // Creation + Assignment
        card.DomainEvents.Last().Should().BeOfType<CardAssignedToStudentDomainEvent>();
    }

    [Fact]
    public void AssignToStudent_WhenAlreadyAssigned_ShouldThrowException()
    {
        // Arrange
        var card = CreateValidCard();
        var firstStudentId = StudentId.CreateUnique();
        var secondStudentId = StudentId.CreateUnique();
        card.AssignToStudent(firstStudentId);

        // Act & Assert
        var act = () => card.AssignToStudent(secondStudentId);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Card is already assigned to a student");
    }

    [Fact]
    public void AssignToStudent_WhenCardIsInactive_ShouldThrowException()
    {
        // Arrange
        var card = CreateValidCard();
        card.ChangeStatus(CardStatus.Inactive);
        var studentId = StudentId.CreateUnique();

        // Act & Assert
        var act = () => card.AssignToStudent(studentId);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot assign an inactive card");
    }

    [Fact]
    public void UnassignFromStudent_WhenAssigned_ShouldUnassignCard()
    {
        // Arrange
        var card = CreateValidCard();
        var studentId = StudentId.CreateUnique();
        card.AssignToStudent(studentId);

        // Act
        card.UnassignFromStudent();

        // Assert
        card.IsAssigned.Should().BeFalse();
        card.AssignedStudentId.Should().BeNull();
        card.CurrentAssignment.Should().NotBeNull();
        card.CurrentAssignment!.IsActive.Should().BeFalse();
        card.DomainEvents.Should().HaveCount(3); // Creation + Assignment + Unassignment
        card.DomainEvents.Last().Should().BeOfType<CardUnassignedFromStudentDomainEvent>();
    }

    [Fact]
    public void UnassignFromStudent_WhenNotAssigned_ShouldThrowException()
    {
        // Arrange
        var card = CreateValidCard();

        // Act & Assert
        var act = () => card.UnassignFromStudent();
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Card is not currently assigned");
    }

    [Fact]
    public void ReportLost_ShouldChangeStatusToLost()
    {
        // Arrange
        var card = CreateValidCard();

        // Act
        card.ReportLost();

        // Assert
        card.Status.Should().Be(CardStatus.Lost);
        card.IsActive.Should().BeFalse();
    }

    [Fact]
    public void ChangeStatus_ToInactiveWhenAssigned_ShouldUnassignCard()
    {
        // Arrange
        var card = CreateValidCard();
        var studentId = StudentId.CreateUnique();
        card.AssignToStudent(studentId);

        // Act
        card.ChangeStatus(CardStatus.Inactive);

        // Assert
        card.Status.Should().Be(CardStatus.Inactive);
        card.IsAssigned.Should().BeFalse();
        card.IsActive.Should().BeFalse();
        card.DomainEvents.Should().HaveCount(3); // Creation + Assignment + Unassignment
    }

    private static Card CreateValidCard()
    {
        return Card.Create(CardType.Student, DateTime.UtcNow.AddYears(1));
    }
}
