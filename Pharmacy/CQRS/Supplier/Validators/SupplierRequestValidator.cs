using FluentValidation;
using Pharmacy.Domain.Models.Deliver;

namespace Pharmacy.CQRS.Supplier.Validators;

public class SupplierRequestValidator : AbstractValidator<SupplierRequest>
{
    public SupplierRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Supplier name is required");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Supplier phone number is required");

        RuleFor(x => x.Address)
            .NotNull()
            .NotEmpty()
            .WithMessage("Supplier address is required");

        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("Supplier email is required")
            .EmailAddress()
            .WithMessage("Supplier email is invalid");

        RuleFor(x => x.VehicleNumber)
            .NotNull()
            .NotEmpty()
            .WithMessage("Supplier vehicleNumber  is required");
    }
}