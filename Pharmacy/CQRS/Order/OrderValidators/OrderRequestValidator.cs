using FluentValidation;
using Pharmacy.Domain.Models.Order.DTOs.Request;

namespace Pharmacy.CQRS.Order.OrderValidators;

public class OrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public OrderRequestValidator()
    {
        RuleFor(x => x.OrderType)
            .IsInEnum()
            .WithMessage("Invalid order type.");
    }
}