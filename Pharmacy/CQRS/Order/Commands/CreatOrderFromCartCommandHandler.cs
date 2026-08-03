using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Cart.Commands;
using Pharmacy.CQRS.Order.Models;
using Pharmacy.CQRS.Order.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Messages.Events;
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
    public async Task<List<OrderResponseForCustomer>> Handle(CreateOrderFromCartCommand request,
        CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(x => x.Customer)
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId, cancellationToken);
        if (cart == null || cart.CartItems.Count == 0)
        {
            throw new RecourseNotFoundException($"Cart is empty");
        }

        var productIds = cart.CartItems
            .Select(x => x.ProductId)
            .ToList();
        var products = await dbContext.Products
            .Where(x => productIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);
        var preparedCartItems = new List<PreparedOrderItem>();
        var orders = new List<Models.Order>();
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

            preparedCartItems.Add(new PreparedOrderItem()
            {
                Product = product,
                Quantity = item.Quantity
            });
        }

        var pharmacyGroup = preparedCartItems
            .GroupBy(x => x.Product.PharmacyId)
            .ToList();
        var pharmacyIds = pharmacyGroup
            .Select(x => x.Key)
            .ToList();
        var pharmacies = await dbContext.Pharmacies
            .Where(x => pharmacyIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);
        foreach (var pharmacy in pharmacyGroup)
        {
            var order = new Models.Order
            {
                CustomerId = cart.CustomerId,
                PharmacyId = pharmacy.Key,
                OrderType = request.OrderType,
                OrderStatus = OrderStatus.Pending,
                Address = cart.Customer.Address,
                TotalAmount = cart.TotalAmount,
            };
            await dbContext.Orders
                .AddAsync(order, cancellationToken);
            foreach (var preparedOrderItem in pharmacy)
            {
                var orderItem = new OrderItem
                {
                    ProductId = preparedOrderItem.Product.Id,
                    Quantity = preparedOrderItem.Quantity,
                    Price = preparedOrderItem.Product.SalePrice,
                    TotalPrice = preparedOrderItem.Product.SalePrice * preparedOrderItem.Quantity
                };
                order.OrderItems.Add(orderItem);

                preparedOrderItem.Product.Stock -= preparedOrderItem.Quantity;
            }

            var totalAmount = order.TotalAmount = order.OrderItems.Sum(x => x.TotalPrice);
            decimal deliveryFee = 0;
            if (request.OrderType is OrderType.Deliver)
            {
                if (!pharmacies.TryGetValue(pharmacy.Key, out var currentPharmacy))
                {
                    throw new RecourseNotFoundException($"Pharmacy with id {pharmacy.Key} not found");
                }

                var routeRequest = new RoutesApiRequest()
                {
                    StartLat = currentPharmacy.Latitude,
                    StartLng = currentPharmacy.Longitude,
                    FinishLat = request.CustomerLatitude,
                    FinishLng = request.CustomerLongitude,
                };
                var distanceKm = await routesService.CalculateRouteAsync(routeRequest);
                deliveryFee = deliveryFeeByDistance.CalCulateDeliveryFee(distanceKm.DistanceKm);

                order.TotalAmount = deliveryFee + totalAmount;
            }

            order.DeliveryFee = deliveryFee;
            orders.Add(order);
            await dbContext.SaveChangesAsync(cancellationToken);
            await publishEndpoint.Publish(new OrderCreatedEvent()
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                DeliveryFee = deliveryFee,
                TotalAmount = order.TotalAmount,
            }, cancellationToken);
        }

        await mediator.Send(new ClearCartCommand(request.CustomerId), cancellationToken);

        return mapper.Map<List<OrderResponseForCustomer>>(orders);
    }
}