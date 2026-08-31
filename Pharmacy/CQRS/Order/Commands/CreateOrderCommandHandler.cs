using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Order.Mapper;
using Pharmacy.CQRS.Order.Models;
using Pharmacy.CQRS.Order.Models.DTOs.Request;
using Pharmacy.CQRS.Order.Models.DTOs.Response;
using Pharmacy.Event.Events;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Services.DeliveryFee;
using Pharmacy.Services.GoogleMaps;

namespace Pharmacy.CQRS.Order.Commands;

public record CreateOrderCommand(
    long CustomerId,
    double? NewCustomerLatitude,
    double? NewCustomerLongitude,
    string? NewCustomerAddress,
    CreateOrderRequest Request) : IRequest<List<OrderResponseForCustomer>>;

public class CreateOrderCommandHandler(
    IPublishEndpoint publishEndpoint,
    IApplicationDbContext dbContext,
    IRoutesService routesService,
    IDeliveryFeeByDistance deliveryFeeByDistance) : IRequestHandler<CreateOrderCommand, List<OrderResponseForCustomer>>
{
    public async Task<List<OrderResponseForCustomer>> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
            .FindAsync([request.CustomerId],
                cancellationToken);
        if (customer is null)
        {
            throw new ResourceNotFoundException("Customer not found");
        }

        var latitude = request.NewCustomerLatitude ?? customer.Latitude;
        var longitude = request.NewCustomerLongitude ?? customer.Longitude;
        var address = request.NewCustomerAddress ?? customer.Address;


        var productIds = request.Request.OrderItemRequest
            .Select(x => x.ProductId)
            .ToList();

        var products = await dbContext.Products
            .Include(x => x.ProductBatches)
            .Where(x => productIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        var preparedOrderItems = new List<PreparedOrderItem>();

        var orders = new List<OrderEntity>();
        foreach (var orderItemRequest in request.Request.OrderItemRequest)
        {
            if (!products.TryGetValue(orderItemRequest.ProductId, out var product))
            {
                throw new ResourceNotFoundException("Product not found");
            }

            if (product.Stock < orderItemRequest.Quantity)
            {
                throw new BusinessException(
                    $"Insufficient stock. Available: {product.Stock}");
            }

            preparedOrderItems.Add(new PreparedOrderItem
            {
                ProductEntity = product,
                Quantity = orderItemRequest.Quantity
            });
        }

        var pharmacyGroup = preparedOrderItems
            .GroupBy(x => x.ProductEntity.PharmacyId)
            .ToList();

        var pharmacyIds = pharmacyGroup.Select(x => x.Key)
            .ToList();

        var pharmacies = await dbContext.Pharmacies
            .Where(x => pharmacyIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        foreach (var pharmacy in pharmacyGroup)
        {
            var order = OrderMappers.ToOrder(request.Request);
            order.OrderStatus = OrderStatus.Pending;
            order.CustomerEntityId = request.CustomerId;
            order.PharmacyId = pharmacy.Key;
            order.Address = address;

            await dbContext.Orders
                .AddAsync(order, cancellationToken);

            foreach (var preparedOrderItem in pharmacy)
            {
                var existingOrderItem = order.OrderItems
                    .FirstOrDefault(x => x.ProductEntityId == preparedOrderItem.ProductEntity.Id);

                if (existingOrderItem != null)
                {
                    existingOrderItem.Quantity += preparedOrderItem.Quantity;
                    existingOrderItem.TotalPrice =
                        existingOrderItem.Quantity * preparedOrderItem.ProductEntity.SalePrice;
                }
                else
                {
                    var orderItem = new OrderItemEntity
                    {
                        ProductEntityId = preparedOrderItem.ProductEntity.Id,
                        Price = preparedOrderItem.ProductEntity.SalePrice,
                        Quantity = preparedOrderItem.Quantity,
                        TotalPrice = preparedOrderItem.Quantity * preparedOrderItem.ProductEntity.SalePrice,
                    };
                    order.OrderItems.Add(orderItem);
                }

                var productBatches = preparedOrderItem.ProductEntity.ProductBatches
                    .Where(x => x.ProductEntityId == preparedOrderItem.ProductEntity.Id &&
                                x is { Quantity: > 0, IsActive: true })
                    .OrderBy(x => x.ExpiryDate)
                    .ToList();

                var requestQuantity = preparedOrderItem.Quantity;

                foreach (var productBatch in productBatches)
                {
                    if (requestQuantity != 0)
                    {
                        var branchQuantity = productBatch.Quantity;

                        if (requestQuantity > branchQuantity)
                        {
                            productBatch.Quantity -= branchQuantity;
                            requestQuantity -= branchQuantity;
                            productBatch.IsActive = false;
                        }
                        else
                        {
                            productBatch.Quantity -= requestQuantity;
                            requestQuantity = 0;
                            if (productBatch.Quantity == 0)
                            {
                                productBatch.IsActive = false;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                preparedOrderItem.ProductEntity.Stock -= preparedOrderItem.Quantity;
            }

            var totalAmount = order.OrderItems.Sum(x => x.TotalPrice);

            decimal deliveryFee = 0;
            if (request.Request.OrderType is OrderType.Deliver)
            {
                if (!pharmacies.TryGetValue(pharmacy.Key, out var currentPharmacy))
                {
                    throw new ResourceNotFoundException($"Pharmacy with id {pharmacy.Key} not found");
                }

                var routeRequest = new RoutesApiRequest
                {
                    StartLat = currentPharmacy.Latitude,
                    StartLng = currentPharmacy.Longitude,
                    FinishLat = latitude,
                    FinishLng = longitude
                };

                var distanceKm = await routesService.CalculateRouteAsync(routeRequest);
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
                    Address = address,
                    DeliveryFee = deliveryFee,
                    TotalAmount = order.TotalAmount,
                    Email = customer.Email
                },
                cancellationToken);
        }

        return OrderMappers.ToListOrderResponseForCustomers(orders);
    }
}