using FluentValidation;
using Pharmacy.Domain.Models.Deliver.DTOs.Request;

namespace Gateway.Validators.Deliver;

public class DeliverRequestValidator : AbstractValidator<DeliverRequest>
{
    public DeliverRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Name is required");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required");

        RuleFor(x => x.Address)
            .NotNull()
            .NotEmpty()
            .WithMessage("Address is required");

        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid Email is format");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters");
    }
}