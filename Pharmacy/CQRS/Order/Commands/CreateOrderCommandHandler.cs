using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Order.Models;
using Pharmacy.CQRS.Order.Models.DTOs.Request;
using Pharmacy.CQRS.Order.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Messages.Events;
using Pharmacy.Models.Domain;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Services.DeliveryFee;
using Pharmacy.Services.GoogleMaps;

namespace Pharmacy.CQRS.Order.Commands;

public record CreateOrderCommand(
    long CustomerId,
    double CustomerLatitude,
    double CustomerLongitude,
    OrderRequest Request) : IRequest<List<OrderResponseForCustomer>>;

public class CreateOrderCommandHandler(
    IMapper mapper,
    IPublishEndpoint publishEndpoint,
    IApplicationDbContext dbContext,
    IRoutesService routesService,
    IDeliveryFeeByDistance deliveryFeeByDistance) : IRequestHandler<CreateOrderCommand, List<OrderResponseForCustomer>>
{
    public async Task<List<OrderResponseForCustomer>> Handle(CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
            .FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken);
        if (customer is null)
        {
            throw new RecourseNotFoundException("Customer not found");
        }

        var productIds = request.Request.OrderItems
            .Select(x => x.ProductId)
            .ToList();
        var products = await dbContext.Products
            .Include(x => x.ProductBatches)
            .Where(x => productIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);
        var preparedOrderItems = new List<PreparedOrderItem>();
        var orders = new List<Models.Order>();
        foreach (var orderItemRequest in request.Request.OrderItems)
        {
            if (!products.TryGetValue(orderItemRequest.ProductId, out var product))
            {
                throw new RecourseNotFoundException("Product not found");
            }

            if (product.Stock < orderItemRequest.Quantity)
            {
                throw new BusinessException(
                    $"Insufficient stock. Available: {product.Stock}");
            }

            preparedOrderItems.Add(new PreparedOrderItem
            {
                Product = product,
                Quantity = orderItemRequest.Quantity
            });
        }

        var pharmacyGroup = preparedOrderItems
            .GroupBy(x => x.Product.PharmacyId)
            .ToList();
        var pharmacyIds = pharmacyGroup.Select(x => x.Key)
            .ToList();
        var pharmacies = await dbContext.Pharmacies
            .Where(x => pharmacyIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);
        foreach (var pharmacy in pharmacyGroup)
        {
            var order = mapper.Map<Models.Order>(request.Request);

            order.OrderStatus = OrderStatus.Pending;
            order.CustomerId = request.CustomerId;
            order.PharmacyId = pharmacy.Key;
            order.Address = customer.Address;
            await dbContext.Orders
                .AddAsync(order, cancellationToken);


            foreach (var preparedOrderItem in pharmacy)
            {
                var existingOrderItem = order.OrderItems
                    .FirstOrDefault(x => x.ProductId == preparedOrderItem.Product.Id);
                if (existingOrderItem != null)
                {
                    existingOrderItem.Quantity += preparedOrderItem.Quantity;
                    existingOrderItem.TotalPrice = existingOrderItem.Quantity * preparedOrderItem.Product.SalePrice;
                }
                else
                {
                    var orderItem = new OrderItem()
                    {
                        ProductId = preparedOrderItem.Product.Id,
                        Price = preparedOrderItem.Product.SalePrice,
                        Quantity = preparedOrderItem.Quantity,
                        TotalPrice = preparedOrderItem.Quantity * preparedOrderItem.Product.SalePrice,
                    };
                    order.OrderItems.Add(orderItem);
                }

                var productBatches = preparedOrderItem.Product.ProductBatches
                    .Where(x => x.ProductId == preparedOrderItem.Product.PharmacyId &&
                                x.Quantity > 0 &&
                                !x.IsActive
                    )
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
                        }
                        else
                        {
                            productBatch.Quantity -= requestQuantity;
                            requestQuantity -= branchQuantity;
                        }
                    }
                }

                preparedOrderItem.Product.Stock -= preparedOrderItem.Quantity;
            }

            var totalAmount = order.OrderItems.Sum(x => x.TotalPrice);
            decimal deliveryFee = 0;
            if (request.Request.OrderType is OrderType.Deliver)
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
                deliveryFee =
                    deliveryFeeByDistance.CalCulateDeliveryFee(distanceKm.DistanceKm);
            }

            order.TotalAmount = deliveryFee + totalAmount;

            order.DeliveryFee = deliveryFee;
            orders.Add(order);
            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            await publishEndpoint.Publish(new OrderCreatedEvent()
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                DeliveryFee = deliveryFee,
                TotalAmount = order.TotalAmount,
            }, cancellationToken);
        }

        return mapper.Map<List<OrderResponseForCustomer>>(orders);
    }
}