using MassTransit;
using MediatR;
using Notifications.CQRS;
using Pharmacy.Event.Events;

namespace Notifications.Consumers;

public class CheckExpiryDateProductConsumer(
    IMediator mediator) : IConsumer<CheckExpiredProductEvent>
{
    public async Task Consume(ConsumeContext<CheckExpiredProductEvent> context)
    {
        var message = context.Message;
        await mediator.Send(new ToPharmacyExpiryProductCommand(
            message.To,
            message.Day,
            message.Count,
            message.TotalPurchasePrice,
            message.TotalSalePrice,
            message.ExpiryDateItems));
    }
}