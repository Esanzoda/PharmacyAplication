using Grpc.Contract.Proto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Order.Mapper;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Order.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Order.Commands;

public record RemoveItemFromOrderCommand(
    long CustomerId,
    long OrderId,
    long OrderItemId) : IRequest<OrderResponseForCustomer>;

public class RemoveItemFromOrderHandler(
    IApplicationDbContext dbContext,
    ProductService.ProductServiceClient productServiceClient)
    : IRequestHandler<RemoveItemFromOrderCommand, OrderResponseForCustomer>
{
    public async Task<OrderResponseForCustomer> Handle(
        RemoveItemFromOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .FirstOrDefaultAsync(x => x.Id == request.OrderId &&
                                      x.CustomerEntityId == request.CustomerId,
                cancellationToken);

        if (order == null)
        {
            throw new ResourceNotFoundException("Order not found");
        }

        if (order.OrderStatus is OrderStatus.Completed or OrderStatus.Cancelled or OrderStatus.Shipped)
        {
            throw new BusinessException("Items cannot be removed from completed, shipped, or cancelled orders");
        }

        var orderItem = await dbContext.OrderItems
            .Include(x => x.OrderItemBatches)
            .FirstOrDefaultAsync(x => x.OrderEntityId == order.Id &&
                                      x.Id == request.OrderItemId,
                cancellationToken);
        if (orderItem == null)
        {
            throw new ResourceNotFoundException($"OrderItem not found");
        }

        var removeItemRequest = orderItem.OrderItemBatches
            .Select(x => new ProductBatchRequest
            {
                ProductBatchId = x.ProductBatchId,
                Quantity = x.Quantity
            });
        var addBackToProductStock = await productServiceClient.CancelOrderAsync(new CancelOrderRequest
        {
            ProductBatchRequest = { removeItemRequest }
        });

        if (!addBackToProductStock.Success)
        {
            throw new BusinessException(addBackToProductStock.Message);
        }

        order.OrderItems.Remove(orderItem);
        var orderItems = await dbContext.OrderItems
            .AnyAsync(x => x.OrderEntityId == request.OrderId &&
                           x.Id != request.OrderItemId,
                cancellationToken);
        if (orderItems)
        {
            order.TotalAmount -= orderItem.TotalPrice;
        }
        else
        {
            order.OrderStatus = OrderStatus.Cancelled;
            order.TotalAmount = 0;
            dbContext.Orders.Remove(order);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return OrderMappers.ToOrderResponseForCustomer(order);
    }
}