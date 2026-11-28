using FluentValidation;
using PharmacyService.Contract.Request;

namespace GatewayService.Api.Validators.Supplier;

public class UpdateSupplierRequestValidator : AbstractValidator<UpdateSupplierRequest>
{
    public UpdateSupplierRequestValidator()
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