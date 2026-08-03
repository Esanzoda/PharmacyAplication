using FluentValidation;
using Pharmacy.CQRS.Order.Models.DTOs.Request;

namespace Pharmacy.CQRS.Order.OrderValidators;

public class OrderRequestValidator : AbstractValidator<OrderRequest>
{
    public OrderRequestValidator()
    {
        RuleFor(x => x.OrderType)
            .IsInEnum()
            .WithMessage("Invalid order type.");
    }
}