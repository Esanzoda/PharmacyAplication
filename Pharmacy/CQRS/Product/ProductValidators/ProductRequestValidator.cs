using FluentValidation;
using Pharmacy.Domain.Models.Product.DTos.Request;

namespace Pharmacy.CQRS.Product.ProductValidators;

public class ProductRequestValidator : AbstractValidator<ProductRequest>
{
    public ProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required");

        RuleFor(x => x.SalePrice)
            .NotEmpty()
            .NotNull()
            .WithMessage("Order price is required")
            .GreaterThan(0)
            .WithMessage("Order price must be greater than 0");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("CategoryId is required")
            .GreaterThan(0)
            .WithMessage("CategoryId must be greater than 0");
    }
}