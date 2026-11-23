using FluentValidation;
using PharmacyService.Api.Application.DTOs.Request;

namespace PharmacyService.Api.Application.CQRS.Supplier.Validators;

public class SupplierUpdateRequestValidator : AbstractValidator<UpdateSupplierRequest>
{
    public SupplierUpdateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Supplier name is required");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Supplier phone number is required");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Supplier email is required")
            .EmailAddress()
            .WithMessage("Supplier email is invalid");

        RuleFor(x => x.VehicleNumber)
            .NotEmpty()
            .WithMessage("Supplier vehicleNumber  is required");
    }
}