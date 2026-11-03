using FluentValidation;
using Pharmacy.Domain.Models.Pharmacy.DTOs.Request;

namespace Gateway.Validators.Pharmacy;

public class PharmacyRequestValidator : AbstractValidator<PharmacyRequest>
{
    public PharmacyRequestValidator()
    {
        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .EmailAddress()
            .WithMessage("Invalid Email is format");
    }
}