using FluentValidation;
using Pharmacy.Domain.Models.Base.Dto.Request;

namespace ProductService.Validator;

public class PaginationRequestValidator : AbstractValidator<PaginationRequest>
{
    public PaginationRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number is invalid");
    }
}