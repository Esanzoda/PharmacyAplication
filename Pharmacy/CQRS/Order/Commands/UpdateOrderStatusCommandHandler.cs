using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Messages.Events;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Order.Commands;

public record UpdateOrderStatusCommand(
    long CustomerId,
    long OrderId,
    UpdateOrderRequest Request) : IRequest<OrderResponse>;

public class UpdateOrderStatusHandler(
    IMapper mapper,
    IPublishEndpoint publishEndpoint,
    IApplicationDbContext dbContext) : IRequestHandler<UpdateOrderStatusCommand, OrderResponse>
{
    public async Task<OrderResponse> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var order = await dbContext.Orders
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId &&
                                      x.Id == request.OrderId, cancellationToken);
        if (order == null)
        {
            throw new RecourseNotFoundException($"Order not found");
        }

//its order.status already changed
        if (order.OrderStatus is OrderStatus.Completed or OrderStatus.Shipped or OrderStatus.Cancelled)
        {
            throw new BusinessException($"Cannot update a {order.OrderStatus} order");
        }


//its new request will cheng orderStatus when order canceled
        if (request.Request.OrderStatus == OrderStatus.Cancelled)
        {
            foreach (var item in order.OrderItems)
            {
                if (item.Product == null)
                {
                    throw new RecourseNotFoundException("Product ot found");
                }

                item.Product.Stock += item.Quantity;
            }

            order.TotalAmount = 0;
        }

        order.OrderStatus = request.Request.OrderStatus;
        await dbContext.SaveChangesAsync(cancellationToken);
        switch (request.Request.OrderStatus)
        {
            case OrderStatus.Cancelled:
                await publishEndpoint.Publish(new OrderCancelledEvent
                {
                    OrderId = order.Id,
                    CustomerId = order.CustomerId,
                    UpdateTime = now
                }, cancellationToken);
                break;
            case OrderStatus.Completed:
                await publishEndpoint.Publish(new OrderCompletedEvent
                {
                    OrderId = order.Id,
                    CustomerId = order.CustomerId,
                    TotalAmount = order.TotalAmount,
                    CompletedAt = now
                }, cancellationToken);
                break;
            case OrderStatus.Shipped:
                await publishEndpoint.Publish(new OrderShippedEventToCeo
                {
                    Count = 1,
                    DateTime = now
                }, cancellationToken);
                break;
        }

        return mapper.Map<OrderResponse>(order);
    }
}