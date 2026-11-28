using FluentValidation;
using PharmacyService.Api.Application.DTOs.Request;

namespace PharmacyService.Api.Validators;

public class UpdateDeliverRequestValidator:AbstractValidator<UpdateDeliverRequest>
{
    public UpdateDeliverRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid Email is format");
    }
}