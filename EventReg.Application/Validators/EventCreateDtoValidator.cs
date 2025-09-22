using EventReg.Application.DTOs;
using FluentValidation;

namespace EventReg.Application.Validators;

public class EventCreateDtoValidator : AbstractValidator<EventCreateDto>
{
    public EventCreateDtoValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty().WithMessage("Event title is required.")
            .MaximumLength(100).WithMessage("Event title cannot exceed 100 characters.");

        RuleFor(e => e.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(e => e.StartDate)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(e => e.EndDate)
            .GreaterThan(e => e.StartDate).WithMessage("End date must be after start date.");

        RuleFor(e => e.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(200).WithMessage("Location cannot exceed 200 characters.");
    }
}
