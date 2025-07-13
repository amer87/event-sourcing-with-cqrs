using FluentAssertions;
using Moq;
using StudentCardAssignment.Application.Cards.Commands.UnassignCard;
using StudentCardAssignment.Application.Common.Interfaces;
using StudentCardAssignment.Domain.Cards;
using StudentCardAssignment.Domain.Cards.Enums;
using StudentCardAssignment.Domain.Cards.ValueObjects;
using StudentCardAssignment.Domain.Students.ValueObjects;
using Xunit;

namespace StudentCardAssignment.Application.Tests.Cards.Commands;

public class UnassignCardCommandHandlerTests
{
    private readonly Mock<IEventSourcedCardRepository> _cardRepositoryMock;
    private readonly UnassignCardCommandHandler _handler;

    public UnassignCardCommandHandlerTests()
    {
        _cardRepositoryMock = new Mock<IEventSourcedCardRepository>();
        _handler = new UnassignCardCommandHandler(_cardRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidAssignedCard_ShouldUnassignCard()
    {
        // Arrange
        var cardId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var command = new UnassignCardCommand(cardId);

        var card = Card.Create(CardType.Student, DateTime.UtcNow.AddYears(1));
        card.AssignToStudent(StudentId.Create(studentId));

        _cardRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<CardId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(card);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CardId.Should().Be(card.CardId.Value);
        result.Status.Should().Be(card.Status.ToString());
        card.IsAssigned.Should().BeFalse();

        _cardRepositoryMock.Verify(x => x.SaveAsync(card, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentCard_ShouldThrowException()
    {
        // Arrange
        var cardId = Guid.NewGuid();
        var command = new UnassignCardCommand(cardId);

        _cardRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<CardId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Card?)null);

        // Act & Assert
        var act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Card with ID '{cardId}' not found");
    }

    [Fact]
    public async Task Handle_WithUnassignedCard_ShouldThrowException()
    {
        // Arrange
        var cardId = Guid.NewGuid();
        var command = new UnassignCardCommand(cardId);

        var card = Card.Create(CardType.Student, DateTime.UtcNow.AddYears(1));

        _cardRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<CardId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(card);

        // Act & Assert
        var act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Card is not currently assigned to any student");
    }
}
