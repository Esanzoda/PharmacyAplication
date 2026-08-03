using MassTransit;
using MediatR;
using Pharmacy.CQRS;
using Pharmacy.CQRS.Notification.Commands;
using Pharmacy.Interfaces;
using Pharmacy.Messages.Events;
using Pharmacy.Services.Message;

namespace Pharmacy.Consumers;

public class OrderCancelledConsumer(
    IApplicationDbContext dbContext,
    ILogger<OrderCancelledConsumer> logger,
    IMediator mediator,
    IMessageService messageService
) : IConsumer<OrderCancelledEvent>
{
    public async Task Consume(ConsumeContext<OrderCancelledEvent> context)
    {
        var message = context.Message;

        logger.LogInformation(
            "Order cancelled: OrderId={OrderId}, CustomerId={CustomerId} UpdateTime={UpdateTime}",
            message.OrderId,
            message.CustomerId,
            message.UpdateTime);


        var user = await dbContext.Customers
            .FindAsync(message.CustomerId);
        if (user != null)
        {
            await mediator.Send(new SendToEmailCustomerOrderCancelledCommand(
                user.Email,
                message.OrderId,
                message.UpdateTime));

            await messageService.SendOrderCancelledAsync(
                user.PhoneNumber,
                message.OrderId,
                message.UpdateTime
            );
        }
    }
}