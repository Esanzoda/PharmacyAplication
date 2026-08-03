using FluentValidation;
using Pharmacy.CQRS.Purchase.Models.DTOs.Request;

namespace Pharmacy.CQRS.Purchase.PurchaseValidators;

public class PurchaseRequestValidator : AbstractValidator<PurchaseRequest>
{
    public PurchaseRequestValidator()
    {
        RuleForEach(x => x.PurchaseItems)
            .SetValidator(new PurchaseItemRequestValidator());
    }
}