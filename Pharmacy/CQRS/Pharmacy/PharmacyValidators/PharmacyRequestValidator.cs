using FluentValidation;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Request;

namespace Pharmacy.CQRS.Pharmacy.PharmacyValidators;

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
            .WithMessage("Address is required");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .NotNull()
            .WithMessage("Address is required");
        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull()
            //.EmailAddress()
            .WithMessage("Address is required");
    }
}