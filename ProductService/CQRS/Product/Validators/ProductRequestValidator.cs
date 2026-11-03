using FluentValidation;
using Pharmacy.Domain.Models.Product.DTos.Request;

namespace ProductService.CQRS.Product.Validators;

public class ProductRequestValidator : AbstractValidator<ProductRequest>
{
    public ProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required");

        RuleFor(x => x.SalePrice)
            .NotEmpty()
            .WithMessage("Sale price is required")
            .GreaterThan(0)
            .WithMessage("Sale price must be greater than 0");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("CategoryId must be greater than 0");
    }
}