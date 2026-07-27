using FluentValidation;
using Pharmacy.Models.Dto.Request;

namespace Pharmacy.CQRS.Product.ProductValidators;

public class ProductRequestValidator : AbstractValidator<ProductRequest>
{
    public ProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required");
        RuleFor(x => x.PurchasePrice)
            .NotEmpty()
            .NotNull()
            .WithMessage("Purchase price is required")
            .GreaterThan(0)
            .WithMessage("Product price must be greater than 0");
        RuleFor(x => x.Price)
            .NotEmpty()
            .NotNull()
            .WithMessage("Order price is required")
            .GreaterThan(0)
            .WithMessage("Order price must be greater than 0");
        RuleFor(x => x.Stock)
            .GreaterThan(0)
            .WithMessage("Stock must be greater than 0");
        RuleFor(x => x.ExpiryDate)
            .NotEmpty()
            .WithMessage("Expiry date is required")
            //   .GreaterThan(DateTime.UtcNow)
            .WithMessage("Expiry date must be in the future");
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("CategoryId is required")
            .GreaterThan(0)
            .WithMessage("CategoryId must be greater than 0");
    }
}