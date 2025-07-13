using FluentValidation;

namespace StudentCardAssignment.Application.Students.Commands.CreateStudent;

public class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100);

        RuleFor(x => x.StudentNumber)
            .NotEmpty()
            .Length(5, 20)
            .Matches("^[A-Za-z0-9]+$")
            .WithMessage("Student number must contain only alphanumeric characters");
    }
}
