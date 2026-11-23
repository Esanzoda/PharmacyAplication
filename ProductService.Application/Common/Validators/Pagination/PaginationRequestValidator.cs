using FluentValidation;
using ProductService.Application.DTOs.Request;

namespace ProductService.Application.Common.Validators.Pagination;

public class PaginationRequestValidator : AbstractValidator<PaginationRequest>
{
    public PaginationRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number is invalid");
    }
}