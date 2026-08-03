using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Order.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Order.Commands;

public record RemoveItemFromOrderCommand(
    long CustomerId,
    long OrderId,
    long ProductId) : IRequest<OrderResponseForCustomer>;

public class RemoveItemFromOrderHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<RemoveItemFromOrderCommand, OrderResponseForCustomer>
{
    public async Task<OrderResponseForCustomer> Handle(RemoveItemFromOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .Include(x => x.OrderItems)
            .FirstOrDefaultAsync(x => x.Id == request.OrderId &&
                                      x.CustomerId == request.CustomerId,
                cancellationToken);
        if (order == null)
        {
            throw new RecourseNotFoundException("Order not found");
        }

        if (order.OrderStatus is OrderStatus.Completed or OrderStatus.Cancelled or OrderStatus.Shipped)
        {
            throw new BusinessException("Items cannot be removed from completed, shipped, or cancelled orders");
        }

        var itemToRemove = order.OrderItems.FirstOrDefault(x => x.ProductId == request.ProductId);
        if (itemToRemove == null)
        {
            throw new RecourseNotFoundException($"OrderItem not found");
        }

        var product = await dbContext.Products
            .FirstOrDefaultAsync(x => x.PharmacyId == order.PharmacyId &&
                                      x.Id == itemToRemove.ProductId, cancellationToken);
        if (product == null)
        {
            throw new RecourseNotFoundException($"Product not found");
        }

        product.Stock += itemToRemove.Quantity;


        order.OrderItems.Remove(itemToRemove);
        dbContext.OrderItems.Remove(itemToRemove);
        if (order.OrderItems.Count == 0)
        {
            dbContext.Orders.Remove(order);
        }

        order.TotalAmount = order.OrderItems.Sum(x => x.TotalPrice);

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<OrderResponseForCustomer>(order);
    }
}