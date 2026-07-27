using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Messages.Events;
using Pharmacy.Models.Domain;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Order.Commands;

public record CreateOrderCommand(
    long CustomerId,
    OrderRequest Request) : IRequest<OrderResponse>;

public class CreateOrderCommandHandler(
    IMapper mapper,
    IPublishEndpoint publishEndpoint,
    IApplicationDbContext dbContext) : IRequestHandler<CreateOrderCommand, OrderResponse>
{
    public async Task<OrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = mapper.Map<Models.Domain.Order>(request.Request);

        order.OrderStatus = OrderStatus.Pending;
        order.CustomerId = request.CustomerId;
        order.CustomerId = request.CustomerId;
        await dbContext.Orders
            .AddAsync(order, cancellationToken);

        var productIds = request.Request.OrderItems
            .Select(x => x.ProductId)
            .ToList();
        var products = await dbContext.Products
            .Where(x => productIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);
        foreach (var item in request.Request.OrderItems)
        {
            if (!products.TryGetValue(item.ProductId, out var product))
            {
                throw new RecourseNotFoundException("Product not found");
            }

            if (product.Stock < item.Quantity)
            {
                throw new BusinessException(
                    $"Insufficient stock. Available: {product.Stock}");
            }

            var existingOrderItem = order.OrderItems
                .FirstOrDefault(x => x.ProductId == item.ProductId);
            if (existingOrderItem != null)
            {
                existingOrderItem.Quantity += item.Quantity;
                existingOrderItem.TotalPrice = existingOrderItem.Quantity * product.Price;
            }
            else
            {
                var orderItem = mapper.Map<OrderItem>(item);
                orderItem.Price = product.Price;
                orderItem.TotalPrice = item.Quantity * product.Price;
                order.OrderItems.Add(orderItem);
            }

            product.Stock -= item.Quantity;
        }

        var totalAmount = order.TotalAmount = order.OrderItems.Sum(x => x.TotalPrice);
        decimal deliverPrice = 0;
        if (request.Request.OrderType is OrderType.Deliver)
        {
            var address = request.Request.Adress;

            if (address is "1" or "2" or "3")
            {
                deliverPrice = 10;
            }
        }

        order.TotalAmount = deliverPrice + totalAmount;
        await dbContext.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(new OrderCreatedEvent()
        {
            OrderId = order.Id,
            CustomerId = order.CustomerId,
            TotalAmount = order.TotalAmount,
        }, cancellationToken);

        return mapper.Map<OrderResponse>(order);
    }
}