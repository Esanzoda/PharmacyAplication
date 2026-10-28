using Grpc.Contract.Proto;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Order.Mapper;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Order.DTOs.Response;
using Pharmacy.Event.Events;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Order.Commands;

public record CancelOrderCommand(
    long CustomerId,
    long OrderId,
    string CustomerEmail) : IRequest<OrderResponseForCustomer>;

public class CancelOrderCommandHandler(
    IPublishEndpoint publishEndpoint,
    IApplicationDbContext dbContext,
    ProductService.ProductServiceClient productServiceClient)
    : IRequestHandler<CancelOrderCommand, OrderResponseForCustomer>
{
    public async Task<OrderResponseForCustomer> Handle(
        CancelOrderCommand request,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var order = await dbContext.Orders
            .Include(x => x.OrderItems)
            .FirstOrDefaultAsync(x => x.CustomerEntityId == request.CustomerId &&
                                      x.Id == request.OrderId,
                cancellationToken);

        if (order == null)
        {
            throw new ResourceNotFoundException($"Order not found");
        }

        if (order.OrderStatus is OrderStatus.Completed or
            OrderStatus.Shipped or
            OrderStatus.Cancelled)
        {
            throw new BusinessException($"Cannot cancel a {order.OrderStatus} order");
        }

        var orderItemIds = order.OrderItems.Select(x => x.Id)
            .ToList();
        var orderItemBatch = await dbContext.OrderItemBatches
            .Where(x => orderItemIds.Contains(x.OrderItemId))
            .Select(x => new ProductBatchRequest
                {
                    ProductBatchId = x.ProductBatchId,
                    Quantity = x.Quantity
                }
            )
            .ToListAsync(cancellationToken);
        if (orderItemBatch.Count == 0)
        {
            throw new ResourceNotFoundException(" OrderItemBatch not found");
        }
        
        var cancelOrder = await productServiceClient.CancelOrderAsync(new CancelOrderRequest
        {
            ProductBatchRequest = { orderItemBatch }
        });
        if (!cancelOrder.Success)
        {
            throw new BusinessException(cancelOrder.Message);
        }

        order.OrderStatus = OrderStatus.Cancelled;
        await dbContext.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(new OrderCancelledEvent
        {
            Email = request.CustomerEmail,
            OrderId = order.Id,
            CustomerId = order.CustomerEntityId,
            UpdateTime = now
        }, cancellationToken);

        return OrderMappers.ToOrderResponseForCustomer(order);
    }
}