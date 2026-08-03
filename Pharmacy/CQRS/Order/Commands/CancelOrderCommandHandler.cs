using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Order.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Messages.Events;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Order.Commands;

public record CancelOrderCommand(
    long CustomerId,
    long OrderId,
    OrderStatus OrderStatus) : IRequest<OrderResponseForCustomer>;

public class UpdateOrderStatusHandler(
    IMapper mapper,
    IPublishEndpoint publishEndpoint,
    IApplicationDbContext dbContext) : IRequestHandler<CancelOrderCommand, OrderResponseForCustomer>
{
    public async Task<OrderResponseForCustomer> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        if (request.OrderStatus != OrderStatus.Cancelled)
        {
            throw new BusinessException("Invalid status, you can only canceled order");
        }

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
        if (request.OrderStatus == OrderStatus.Cancelled)
        {
            foreach (var item in order.OrderItems)
            {
                if (item.Product == null)
                {
                    throw new RecourseNotFoundException("Product ot found");
                }

                item.Product.Stock += item.Quantity;
            }
        }

        order.OrderStatus = request.OrderStatus;
        await dbContext.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(new OrderCancelledEvent
        {
            OrderId = order.Id,
            CustomerId = order.CustomerId,
            UpdateTime = now
        }, cancellationToken);


        return mapper.Map<OrderResponseForCustomer>(order);
    }
}