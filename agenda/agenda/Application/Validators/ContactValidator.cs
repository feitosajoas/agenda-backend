using agenda.Models;
using FluentValidation;

namespace agenda.Application.Validators;

public class ContactValidator : AbstractValidator<Contact>
{
    public ContactValidator()
    {
        RuleFor(contact => contact.Name)
            .NotEmpty().WithMessage("The Name field is required.")
            .Length(2, 100).WithMessage("The Name must be between 2 and 100 characters.");

        RuleFor(contact => contact.PhoneNumber)
            .NotEmpty().WithMessage("The PhoneNumber field is required.")
            .Matches(@"^\+?\d+$").WithMessage("The PhoneNumber must contain only numbers and can optionally start with a '+'.");

        RuleFor(contact => contact.Email)
            .NotEmpty().WithMessage("The Email field is required.")
            .EmailAddress().WithMessage("The Email field must contain a valid email address.");
    }
}
