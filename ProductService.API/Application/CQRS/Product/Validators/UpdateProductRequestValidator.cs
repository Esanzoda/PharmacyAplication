using FluentValidation;
using ProductService.Api.Application.DTOs.Request;

namespace ProductService.Api.Application.CQRS.Product.Validators;

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("Category id must be greater than 0");
        RuleFor(x => x.Name)
            .MaximumLength(40)
            .MinimumLength(4)
            .WithMessage("Min length 4 Max length 40");
    }
}