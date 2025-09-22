using EventReg.Application.DTOs;
using FluentValidation;

namespace EventReg.Application.Validators;

public class RegistrationCreateDtoValidator : AbstractValidator<RegistrationCreateDto>
{
    public RegistrationCreateDtoValidator()
    {
        RuleFor(x => x.EventId)
            .GreaterThan(0).WithMessage("EventId must be provided");

        RuleFor(x => x.AttendeeName)
            .NotEmpty().WithMessage("Attendee is required")
            .MaximumLength(100);

        RuleFor(x => x.AttendeeEmail)
            .NotEmpty().EmailAddress().WithMessage("Valid attendee email is required");

        RuleFor(x => x.UserId)
            .GreaterThan(0).When(x => x.UserId.HasValue);
    }
}
