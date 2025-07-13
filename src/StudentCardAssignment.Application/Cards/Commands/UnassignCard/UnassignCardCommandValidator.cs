using FluentValidation;

namespace StudentCardAssignment.Application.Cards.Commands.UnassignCard;

public class UnassignCardCommandValidator : AbstractValidator<UnassignCardCommand>
{
    public UnassignCardCommandValidator()
    {
        RuleFor(x => x.CardId)
            .NotEmpty()
            .WithMessage("Card ID is required");
    }
}
