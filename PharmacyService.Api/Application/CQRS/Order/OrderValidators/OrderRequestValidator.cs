using FluentValidation;
using PharmacyService.Api.Application.DTOs.Request;

namespace PharmacyService.Api.Application.CQRS.Order.OrderValidators;

public class OrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public OrderRequestValidator()
    {
        RuleFor(x => x.OrderType)
            .IsInEnum()
            .WithMessage("Invalid order type.");
    }
}