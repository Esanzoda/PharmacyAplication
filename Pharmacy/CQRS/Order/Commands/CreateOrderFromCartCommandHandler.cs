using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Cart.Commands;
using Pharmacy.CQRS.Order.Models;
using Pharmacy.CQRS.Order.Models.DTOs.Response;
using Pharmacy.Event.Events;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Services.DeliveryFee;
using Pharmacy.Services.GoogleMaps;

namespace Pharmacy.CQRS.Order.Commands;

public record CreateOrderFromCartCommand(
    long CustomerId,
    OrderType OrderType,
    double CustomerLatitude,
    double CustomerLongitude) : IRequest<List<OrderResponseForCustomer>>;

public class CreatOrderFromCartHandler(
    IMapper mapper,
    IApplicationDbContext dbContext,
    IMediator mediator,
    IPublishEndpoint publishEndpoint,
    IRoutesService routesService,
    IDeliveryFeeByDistance deliveryFeeByDistance)
    : IRequestHandler<CreateOrderFromCartCommand, List<OrderResponseForCustomer>>
{
    public async Task<List<OrderResponseForCustomer>> Handle(
        CreateOrderFromCartCommand request,
        CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(x => x.CustomerEntity)
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.CustomerEntityId == request.CustomerId,
                cancellationToken);

        if (cart == null)
        {
            throw new ResourceNotFoundException("Cart not found");
        }

        if (cart.CartItems.Count == 0)
        {
            throw new BusinessException("Cart is empty");
        }

        var productIds = cart.CartItems
            .Select(x => x.ProductEntityId)
            .ToList();

        var products = await dbContext.Products
            .Where(x => productIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id,
                cancellationToken);

        var preparedCartItems = new List<PreparedOrderItem>();

        var orders = new List<OrderEntity>();

        foreach (var item in cart.CartItems)
        {
            if (!products.TryGetValue(item.ProductEntityId, out var product))
            {
                throw new ResourceNotFoundException("Product not found");
            }

            if (product.Stock < item.Quantity)
            {
                throw new BusinessException(
                    $"Insufficient stock for product {product.Name}: available {product.Stock}, requested {item.Quantity}");
            }

            preparedCartItems.Add(new PreparedOrderItem()
            {
                ProductEntity = product,
                Quantity = item.Quantity
            });
        }

        var pharmacyGroup = preparedCartItems
            .GroupBy(x => x.ProductEntity.PharmacyId)
            .ToList();

        var pharmacyIds = pharmacyGroup
            .Select(x => x.Key)
            .ToList();

        var pharmacies = await dbContext.Pharmacies
            .Where(x => pharmacyIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        foreach (var pharmacy in pharmacyGroup)
        {
            var order = new OrderEntity
            {
                CustomerEntityId = cart.CustomerEntityId,
                PharmacyId = pharmacy.Key,
                OrderType = request.OrderType,
                OrderStatus = OrderStatus.Pending,
                Address = cart.CustomerEntity.Address
            };

            await dbContext.Orders
                .AddAsync(order, cancellationToken);

            foreach (var preparedOrderItem in pharmacy)
            {
                var orderItem = new OrderItemEntity
                {
                    ProductEntityId = preparedOrderItem.ProductEntity.Id,
                    Quantity = preparedOrderItem.Quantity,
                    Price = preparedOrderItem.ProductEntity.SalePrice,
                    TotalPrice = preparedOrderItem.ProductEntity.SalePrice * preparedOrderItem.Quantity
                };

                order.OrderItems.Add(orderItem);

                preparedOrderItem.ProductEntity.Stock -= preparedOrderItem.Quantity;
            }

            var totalAmount = order.TotalAmount = order.OrderItems.Sum(x => x.TotalPrice);

            decimal deliveryFee = 0;
            if (request.OrderType is OrderType.Deliver)
            {
                if (!pharmacies.TryGetValue(pharmacy.Key, out var currentPharmacy))
                {
                    throw new ResourceNotFoundException($"Pharmacy with id {pharmacy.Key} not found");
                }

                var routeRequest = new RoutesApiRequest()
                {
                    StartLat = currentPharmacy.Latitude,
                    StartLng = currentPharmacy.Longitude,
                    FinishLat = request.CustomerLatitude,
                    FinishLng = request.CustomerLongitude,
                };

                var distanceKm = await routesService
                    .CalculateRouteAsync(routeRequest);

                deliveryFee = deliveryFeeByDistance
                    .CalculateDeliveryFee(distanceKm.DistanceKm);
            }

            order.TotalAmount = deliveryFee + totalAmount;

            order.DeliveryFee = deliveryFee;
            orders.Add(order);

            await dbContext.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(new OrderCreatedEvent
            {
                OrderId = order.Id,
                CustomerId = order.CustomerEntityId,
                DeliveryFee = deliveryFee,
                TotalAmount = order.TotalAmount,
                Email = cart.CustomerEntity.Email,
                CreatedAt = order.CreatedAt,
                Address = order.Address
            }, cancellationToken);
        }

        await mediator.Send(new ClearCartCommand(request.CustomerId), cancellationToken);

        return mapper.Map<List<OrderResponseForCustomer>>(orders);
    }
}