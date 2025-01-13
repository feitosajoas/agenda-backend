using agenda.Application.Commands;
using agenda.Models;
using FluentValidation;

namespace agenda.Application.Validators;

public class UserValidator : AbstractValidator<UserCommand>
{
    public UserValidator()
    {
        RuleFor(user => user.Name)
    .NotEmpty().WithMessage("The name is required.")
    .Length(3, 50).WithMessage("The name must be between 3 and 50 characters long.");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("The password is required.")
            .MinimumLength(8).WithMessage("The password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("The password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("The password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("The password must contain at least one number.")
            .Matches("[^a-zA-Z0-9]").WithMessage("The password must contain at least one special character.");

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("The email is required.")
            .EmailAddress().WithMessage("The email must be valid.");
    }
}
