using FluentAssertions;
using Moq;
using StudentCardAssignment.Application.Common.Interfaces;
using StudentCardAssignment.Application.Students.Commands.CreateStudent;
using StudentCardAssignment.Domain.Students;
using StudentCardAssignment.Domain.Students.ValueObjects;
using Xunit;

namespace StudentCardAssignment.Application.Tests.Students.Commands;

public class CreateStudentCommandHandlerTests
{
    private readonly Mock<IEventSourcedStudentRepository> _studentRepositoryMock;
    private readonly CreateStudentCommandHandler _handler;

    public CreateStudentCommandHandlerTests()
    {
        _studentRepositoryMock = new Mock<IEventSourcedStudentRepository>();

        _handler = new CreateStudentCommandHandler(
            _studentRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateStudentAndReturnResult()
    {
        // Arrange
        var command = new CreateStudentCommand(
            "John",
            "Doe",
            "john.doe@example.com",
            "ST12345");

        _studentRepositoryMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Student?)null);

        _studentRepositoryMock
            .Setup(x => x.GetByStudentNumberAsync(It.IsAny<StudentNumber>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Student?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.Email.Should().Be("john.doe@example.com");
        result.StudentNumber.Should().Be("ST12345");

        _studentRepositoryMock.Verify(x => x.SaveAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithExistingEmail_ShouldThrowException()
    {
        // Arrange
        var command = new CreateStudentCommand(
            "John",
            "Doe",
            "john.doe@example.com",
            "ST12345");

        var existingStudent = Student.Create(
            "Jane",
            "Smith",
            Email.Create("john.doe@example.com"),
            StudentNumber.Create("ST99999"));

        _studentRepositoryMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingStudent);

        // Act & Assert
        var act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Student with email 'john.doe@example.com' already exists");
    }

    [Fact]
    public async Task Handle_WithExistingStudentNumber_ShouldThrowException()
    {
        // Arrange
        var command = new CreateStudentCommand(
            "John",
            "Doe",
            "john.doe@example.com",
            "ST12345");

        var existingStudent = Student.Create(
            "Jane",
            "Smith",
            Email.Create("jane.smith@example.com"),
            StudentNumber.Create("ST12345"));

        _studentRepositoryMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Student?)null);

        _studentRepositoryMock
            .Setup(x => x.GetByStudentNumberAsync(It.IsAny<StudentNumber>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingStudent);

        // Act & Assert
        var act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Student with number 'ST12345' already exists");
    }
}
