using FluentValidation;
using Pharmacy.Domain.Models.Pharmacy.DTOs.Request;

namespace Gateway.Validators.Pharmacy;

public class PharmacyRequestValidator : AbstractValidator<PharmacyRequest>
{
    public PharmacyRequestValidator()
    {
        RuleFor(x => x.Address)
            .NotEmpty()
            .NotNull()
            .WithMessage("Address is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage("Name is required");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .NotNull()
            .WithMessage("Phone number is required");

        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull()
            .WithMessage("Phone number is required")
            .EmailAddress()
            .WithMessage("Invalid Email is format");
    }
}