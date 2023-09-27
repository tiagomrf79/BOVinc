using FarmsAPI.Models;
using FluentValidation;

namespace FarmsAPI.Validations;

public class HerdValidator : AbstractValidator<Herd>
{
    public HerdValidator()
    {
        RuleFor(h => h.Name)
            .NotNull().NotEmpty().WithMessage("Value is required.")
            .MaximumLength(100).WithMessage("Value must not exceed 100 characters.");

        RuleFor(h => h.Address)
            .MaximumLength(200).WithMessage("Value must not exceed 200 characters.");

        RuleFor(h => h.City)
            .MaximumLength(50).WithMessage("Value must not exceed 50 characters.");

        RuleFor(h => h.Region)
            .MaximumLength(50).WithMessage("Value must not exceed 50 characters.");

        RuleFor(h => h.Country)
            .MaximumLength(50).WithMessage("Value must not exceed 50 characters.");
    }
}
