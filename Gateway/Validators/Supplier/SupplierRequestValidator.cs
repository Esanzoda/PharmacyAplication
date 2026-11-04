using FluentValidation;
using Pharmacy.Domain.Models.Deliver;

namespace Gateway.Validators.Supplier;

public class SupplierRequestValidator : AbstractValidator<SupplierRequest>
{
    public SupplierRequestValidator()
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
            .WithMessage("Supplier email is invalid");

        RuleFor(x => x.VehicleNumber)
            .NotEmpty()
            .WithMessage("Supplier vehicleNumber  is required");
    }
}