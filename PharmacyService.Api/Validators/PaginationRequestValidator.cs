using FluentValidation;
using PharmacyService.Api.Application.DTOs.Request;

namespace PharmacyService.Api.Validators;

public class PaginationRequestValidator : AbstractValidator<PaginationRequest>
{
    public PaginationRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number is invalid");
    }
}