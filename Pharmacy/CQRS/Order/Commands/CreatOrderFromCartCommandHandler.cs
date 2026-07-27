using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Cart.Commands;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Messages.Events;
using Pharmacy.Models.Domain;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Order.Commands;

public record CreateOrderFromCartCommand(
    long CustomerId,
    OrderType OrderType,
    string Address) : IRequest<OrderResponse>;

public class CreatOrderFromCartHandler(
    IMapper mapper,
    IApplicationDbContext dbContext,
    IMediator mediator,
    IPublishEndpoint publishEndpoint)
    : IRequestHandler<CreateOrderFromCartCommand, OrderResponse>
{
    public async Task<OrderResponse> Handle(CreateOrderFromCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(x => x.Customer)
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId, cancellationToken);
        if (cart == null || cart.CartItems.Count == 0)
        {
            throw new RecourseNotFoundException($"Cart is empty");
        }

        var newAddress = string.IsNullOrEmpty(request.Address) ? cart.Customer.Address : request.Address;

        var order = new Models.Domain.Order
        {
            CustomerId = cart.CustomerId,
            OrderType = request.OrderType,
            OrderStatus = OrderStatus.Pending,
            Address = newAddress,
            TotalAmount = cart.TotalAmount,
        };
        await dbContext.Orders
            .AddAsync(order, cancellationToken);

        var productIds = cart.CartItems
            .Select(x => x.ProductId)
            .ToList();
        var products = await dbContext.Products
            .Where(x => productIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);
        foreach (var item in cart.CartItems)
        {
            if (!products.TryGetValue(item.ProductId, out var product))
            {
                throw new RecourseNotFoundException("Product not found");
            }

            if (product.Stock < item.Quantity)
            {
                throw new BusinessException(
                    $"Insufficient stock for product {product.Name}: available {product.Stock}, requested {item.Quantity}");
            }

            var orderItem = new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = product.Price,
                TotalPrice = product.Price * item.Quantity
            };
            order.OrderItems.Add(orderItem);

            product.Stock -= item.Quantity;
        }


        var totalAmount = order.TotalAmount = order.OrderItems.Sum(x => x.TotalPrice);
        decimal deliverPrice = 0;
        if (request.OrderType is OrderType.Deliver)
        {
            var address = newAddress;

            if (address is "1" or "2" or "3")
            {
                deliverPrice = 10;
            }

            order.TotalAmount = deliverPrice + totalAmount;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        await mediator.Send(new ClearCartCommand(request.CustomerId), cancellationToken);
        await publishEndpoint.Publish(new OrderCreatedEvent()
        {
            OrderId = order.Id,
            CustomerId = order.CustomerId,
            DeliverPrice = deliverPrice,
            TotalAmount = order.TotalAmount,
        }, cancellationToken);
        return mapper.Map<OrderResponse>(order);
    }
}