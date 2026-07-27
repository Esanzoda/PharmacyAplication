using FluentValidation;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Request;

namespace Pharmacy.CQRS.Customer.CustomerValidators;

public class CustomerRequestValidator : AbstractValidator<CustomerRequest>
{
    public CustomerRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage("Name is requared");

        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull()
            .WithMessage("Email is required")
            // .EmailAddress()
            .WithMessage("Invalid email format");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .NotNull()
            .WithMessage("Invalid phone number format");

        RuleFor(x => x.Address)
            .NotEmpty()
            .NotNull()
            .WithMessage("Invalid adress format");

        RuleFor(x => x.Role)
            .Equal(Role.Customer)
            .WithMessage("You can create only customer role.");
    }
/*
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters");*/
}