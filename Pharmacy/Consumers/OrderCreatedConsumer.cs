using MassTransit;
using MediatR;
using Pharmacy.CQRS.Notification.Commands;
using Pharmacy.Interfaces;
using Pharmacy.Messages.Events;

namespace Pharmacy.Consumers;

public class OrderCreatedConsumer(
    ILogger<OrderCreatedConsumer> logger,
    IMediator mediator,
    IApplicationDbContext dbContext) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;

        logger.LogInformation(
            "Order created: OrderId={OrderId}, CustomerId={CustomerId}, DeliveryFee{DeliveryFee} Total={Total} , CreatedAt={CreatedAt}",
            message.OrderId,
            message.CustomerId,
            message.DeliveryFee,
            message.TotalAmount,
            message.CreatedAt);

        var user = await dbContext.Customers
            .FindAsync(message.CustomerId);
        if (user != null)
        {
            await mediator.Send(new SendToEmailCustomerOrderCreateCommand(
                user.Email,
                message.OrderId,
                message.TotalAmount,
                message.DeliveryFee,
                message.CreatedAt));
        }
    }
}