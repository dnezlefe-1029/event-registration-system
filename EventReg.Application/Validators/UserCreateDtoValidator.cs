using EventReg.Application.DTOs;
using FluentValidation;

namespace EventReg.Application.Validators;

public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
{
    public UserCreateDtoValidator()
    {
        RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Name is required")
               .MaximumLength(100);

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6) 
            .WithMessage("Password must be at least 6 characters long");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Valid email is required");

        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(r => r == "Admin" || r == "Attendee")
            .WithMessage("Role must be Admin or Attendee");
    }
}
