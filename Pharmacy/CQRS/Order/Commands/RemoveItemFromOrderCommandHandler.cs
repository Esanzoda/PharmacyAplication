using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Order.Commands;

public record RemoveItemFromOrderCommand(
    long CustomerId,
    long OrderId,
    long ItemId) : IRequest<OrderResponse>;

public class RemoveItemFromOrderHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<RemoveItemFromOrderCommand, OrderResponse>
{
    public async Task<OrderResponse> Handle(RemoveItemFromOrderCommand request, CancellationToken cancellationToken)
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
            throw new BusinessException("Can't remove item completed or cancelled order ");
        }

        var itemToRemove = order.OrderItems.FirstOrDefault(x => x.Id == request.ItemId);
        if (itemToRemove == null)
        {
            throw new RecourseNotFoundException($"OrderItem not found");
        }

        var product = await dbContext.Products
            .FirstOrDefaultAsync(x => x.PharmacyId == itemToRemove.PharmacyId &&
                                      x.Id == itemToRemove.ProductId, cancellationToken);
        if (product == null)
        {
            throw new RecourseNotFoundException($"Product not found");
        }

        product.Stock += itemToRemove.Quantity;


        order.OrderItems.Remove(itemToRemove);
        order.TotalAmount = order.OrderItems.Sum(x => x.TotalPrice);

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<OrderResponse>(order);
    }
}