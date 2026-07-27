using FluentValidation;
using Pharmacy.Models.Dto.Request;

namespace Pharmacy.CQRS.Order.OrderValidators;

public class OrderItemRequestValidator : AbstractValidator<OrderItemRequest>
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