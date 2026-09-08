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
    IApplicationDbContext dbContext) : IRequestHandler<CancelOrderCommand, OrderResponseForCustomer>
{
    public async Task<OrderResponseForCustomer> Handle(
        CancelOrderCommand request,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var order = await dbContext.Orders
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.ProductEntity)
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

        foreach (var item in order.OrderItems)
        {
            if (item.ProductEntity == null)
            {
                throw new ResourceNotFoundException("Product not found");
            }

            item.ProductEntity.Stock += item.Quantity;
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