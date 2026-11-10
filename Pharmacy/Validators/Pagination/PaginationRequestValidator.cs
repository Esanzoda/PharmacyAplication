using FluentValidation;
using Pharmacy.Domain.Models.Base.Dto.Request;

namespace Pharmacy.Validators.Pagination;

public class PaginationRequestValidator : AbstractValidator<PaginationRequest>
{
    public PaginationRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number is invalid");
    }
}