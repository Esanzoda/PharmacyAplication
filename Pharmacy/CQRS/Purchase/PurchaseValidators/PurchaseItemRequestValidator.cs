using FluentValidation;
using Pharmacy.Domain.Models.PurchaseEntity.DTOs.Request;

namespace Pharmacy.CQRS.Purchase.PurchaseValidators;

public class PurchaseItemRequestValidator : AbstractValidator<PurchaseItemRequest>
{
    public PurchaseItemRequestValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");

        RuleFor(x => x.ProductEntityId)
            .GreaterThan(0)
            .WithMessage("ProductId must be greater than 0");

        RuleFor(x => x.PurchasePrice)
            .NotEmpty()
            .WithMessage("Purchase Price must not be null")
            .GreaterThan(0)
            .WithMessage("Purchase Price must be greater than 0");

        RuleFor(x => x.ExpiryDate)
            .NotEmpty()
            .WithMessage("ExpiryDate must not be null")
            .GreaterThan(DateOnly.FromDateTime(DateTime.UtcNow.Date))
            .WithMessage("Expiry date must be in the future");
    }
}