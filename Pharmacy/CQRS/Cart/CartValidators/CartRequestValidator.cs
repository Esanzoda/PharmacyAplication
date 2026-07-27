using FluentValidation;
using Pharmacy.Models.Dto.Request;

namespace Pharmacy.CQRS.Cart.CartValidators;

public class CartRequestValidator : AbstractValidator<CartRequest>
{
    public CartRequestValidator()
    {
        RuleForEach(x => x.CartItems)
            .SetValidator(new CartItemRequestValidator());
    }
}