using FluentValidation;
using Pharmacy.Models.Dto.Request;

namespace Pharmacy.CQRS.Order.OrderValidators;

public class OrderReservationRequestValidator : AbstractValidator<OrderReservationRequest>
{
    public OrderReservationRequestValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("Customer id must be greater than 0");
    }
}