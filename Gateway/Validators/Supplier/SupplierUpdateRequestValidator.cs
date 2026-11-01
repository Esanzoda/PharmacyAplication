using FluentValidation;
using Pharmacy.Domain.Models.Deliver;

namespace Gateway.Validators.Supplier;

public class SupplierUpdateRequestValidator : AbstractValidator<UpdateSupplierRequest>
{
    public SupplierUpdateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid Email is format");

        RuleFor(x => x.VehicleNumber)
            .NotEmpty()
            .WithMessage("VehicleNumber  is required");
    }
}