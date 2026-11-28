using FluentValidation;
using PharmacyService.Contract.Request;

namespace GatewayService.Api.Validators.Order;

public class OrderItemRequestValidator : AbstractValidator<CreateOrderItemRequest>
{
    public OrderItemRequestValidator()
    {
        RuleFor(order => order.ProductId)
            .GreaterThan(0)
            .WithMessage("Product id must be greater than 0");

        RuleFor(order => order.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");
    }
}