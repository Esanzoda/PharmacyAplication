using FluentValidation;
using Pharmacy.CQRS.Employee.Models.DTOs.Request;

namespace Pharmacy.CQRS.Employee.EmployeeValidators;

public class EmployeeRequestValidator : AbstractValidator<EmployeeRequest>
{
    public EmployeeRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required");
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            // .EmailAddress()
            .WithMessage("Invalid email format");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required");
        RuleFor(x => x.Salary)
            .GreaterThan(0)
            .WithMessage("Salary must be greater than 0");
        /*  RuleFor(x => x.Password)
              .MinimumLength(8)
              .WithMessage("Password must be at least 8 characters");*/
    }
}