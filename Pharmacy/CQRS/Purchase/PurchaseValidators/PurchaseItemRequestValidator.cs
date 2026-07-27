using FluentValidation;
using Pharmacy.Models.Dto.Request;

namespace Pharmacy.CQRS.Purchase.PurchaseValidators;

public class PurchaseItemRequestValidator : AbstractValidator<PurchaseItemRequest>
{
    public PurchaseItemRequestValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");
        /*RuleFor(x=>x.PurchaseId)
            .GreaterThan(0)
            .WithMessage("PurchaseId must be greater than 0");*/
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId must be greater than 0");
        RuleFor(x => x.PurchasePrice)
            .NotNull()
            .WithMessage("Purchase Price must not be null")
            .GreaterThan(0)
            .WithMessage("Purchase Price must be greater than 0");
        RuleFor(x => x.Price)
            .NotNull()
            .WithMessage("Purchase Price must not be null")
            .GreaterThan(0)
            .WithMessage("PurchasePrice must be greater than 0");
        RuleFor(x => x.ExpiryDate)
            .NotNull()
            .WithMessage("ExpiryDate must not be null")
            //    .GreaterThan(DateTime.UtcNow)
            .WithMessage("Expiry date must be in the future");
    }
}