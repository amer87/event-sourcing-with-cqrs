using FluentAssertions;
using StudentCardAssignment.Domain.Students;
using StudentCardAssignment.Domain.Students.ValueObjects;
using StudentCardAssignment.Domain.Students.Events;
using StudentCardAssignment.Domain.Students.Enums;
using Xunit;

namespace StudentCardAssignment.Domain.Tests.Students;

public class StudentTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateStudent()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var email = Email.Create("john.doe@example.com");
        var studentNumber = StudentNumber.Create("ST12345");

        // Act
        var student = Student.Create(firstName, lastName, email, studentNumber);

        // Assert
        student.Should().NotBeNull();
        student.FirstName.Should().Be(firstName);
        student.LastName.Should().Be(lastName);
        student.Email.Should().Be(email);
        student.StudentNumber.Should().Be(studentNumber);
        student.Status.Should().Be(StudentStatus.Active);
        student.IsActive.Should().BeTrue();
        student.GetFullName().Should().Be("John Doe");
        student.DomainEvents.Should().HaveCount(1);
        student.DomainEvents.First().Should().BeOfType<StudentCreatedDomainEvent>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithInvalidFirstName_ShouldThrowException(string firstName)
    {
        // Arrange
        var lastName = "Doe";
        var email = Email.Create("john.doe@example.com");
        var studentNumber = StudentNumber.Create("ST12345");

        // Act & Assert
        var act = () => Student.Create(firstName, lastName, email, studentNumber);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ChangeStatus_WithDifferentStatus_ShouldChangeStatusAndRaiseDomainEvent()
    {
        // Arrange
        var student = CreateValidStudent();
        var newStatus = StudentStatus.Inactive;

        // Act
        student.ChangeStatus(newStatus);

        // Assert
        student.Status.Should().Be(newStatus);
        student.IsActive.Should().BeFalse();
        student.DomainEvents.Should().HaveCount(2); // Creation + Status change
        student.DomainEvents.Last().Should().BeOfType<StudentStatusChangedDomainEvent>();
    }

    [Fact]
    public void ChangeStatus_WithSameStatus_ShouldNotRaiseDomainEvent()
    {
        // Arrange
        var student = CreateValidStudent();
        var currentStatus = student.Status;

        // Act
        student.ChangeStatus(currentStatus);

        // Assert
        student.Status.Should().Be(currentStatus);
        student.DomainEvents.Should().HaveCount(1); // Only creation event
    }

    [Fact]
    public void UpdateProfile_WithValidData_ShouldUpdateProfile()
    {
        // Arrange
        var student = CreateValidStudent();
        var newFirstName = "Jane";
        var newLastName = "Smith";
        var newEmail = Email.Create("jane.smith@example.com");

        // Act
        student.UpdateProfile(newFirstName, newLastName, newEmail);

        // Assert
        student.FirstName.Should().Be(newFirstName);
        student.LastName.Should().Be(newLastName);
        student.Email.Should().Be(newEmail);
        student.UpdatedAt.Should().NotBeNull();
        student.GetFullName().Should().Be("Jane Smith");
    }

    private static Student CreateValidStudent()
    {
        return Student.Create(
            "John",
            "Doe",
            Email.Create("john.doe@example.com"),
            StudentNumber.Create("ST12345"));
    }
}
