using FluentValidation;
using ProductService.Api.Application.DTOs.Request;

namespace ProductService.Api.Validators;

public class ProductRequestValidator : AbstractValidator<ProductRequest>
{
    public ProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required");

        RuleFor(x => x.SalePrice)
            .GreaterThan(0)
            .WithMessage("Sale price must be greater than 0");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("CategoryId must be greater than 0");
    }
}