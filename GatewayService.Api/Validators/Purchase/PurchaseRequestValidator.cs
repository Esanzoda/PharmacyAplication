using FluentValidation;
using PharmacyService.Contract.Request;

namespace GatewayService.Api.Validators.Purchase;

public class PurchaseRequestValidator : AbstractValidator<PurchaseRequest>
{
    public PurchaseRequestValidator()
    {
        RuleForEach(x => x.PurchaseItems)
            .SetValidator(new PurchaseItemRequestValidator());
    }
}