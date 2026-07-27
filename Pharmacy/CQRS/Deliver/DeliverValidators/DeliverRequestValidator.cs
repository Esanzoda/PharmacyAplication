using FluentValidation;
using Pharmacy.Models.Dto.Request;

namespace Pharmacy.CQRS.Deliver.DeliverValidators;

public class DeliverRequestValidator : AbstractValidator<DeliverRequest>
{
    public DeliverRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Deliver name is required");
        RuleFor(x => x.PhoneNumber)
            .NotNull()
            .NotEmpty()
            .WithMessage("Deliver phone number is required");
        RuleFor(x => x.Address)
            .NotNull()
            .NotEmpty()
            .WithMessage("Deliver address is required");
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("Deliver email is required")
            // .EmailAddress()
            .WithMessage("Deliver email is invalid");
    }
}