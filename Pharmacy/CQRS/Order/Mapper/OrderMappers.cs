using Pharmacy.CQRS.Order.Models;
using Pharmacy.CQRS.Order.Models.DTOs.Request;
using Pharmacy.CQRS.Order.Models.DTOs.Response;

namespace Pharmacy.CQRS.Order.Mapper;

public static class OrderMappers
{
    public static OrderEntity ToOrder(CreateOrderRequest request)
    {
        return new OrderEntity
        {
            OrderType = request.OrderType,
            PicKupTime = request.PicKupTime
        };
    }


    private static OrderItemResponse ToOrderItemResponse(OrderItemEntity orderItem)
    {
        return new OrderItemResponse
        {
            Id = orderItem.Id,
            ProductEntityId = orderItem.ProductEntityId,
            Quantity = orderItem.Quantity,
            Price = orderItem.Price,
            TotalPrice = orderItem.TotalPrice
        };
    }

    public static OrderResponseForCustomer ToOrderResponseForCustomer(OrderEntity order)
    {
        return new OrderResponseForCustomer
        {
            Id = order.Id,
            PharmacyId = order.PharmacyId,
            OrderType = order.OrderType,
            Address = order.Address,
            Deliver = order.Deliver,
            OrderStatus = order.OrderStatus,
            DeliveryFee = order.DeliveryFee,
            TotalAmount = order.TotalAmount,
            CreatedAt = order.CreatedAt,
            PicKupTime = order.PicKupTime,
            OrderItemResponses = order.OrderItems
                .Select(ToOrderItemResponse)
                .ToList()
        };
    }

    public static List<OrderResponseForCustomer> ToListOrderResponseForCustomers(List<OrderEntity> orders)
    {
        return orders
            .Select(ToOrderResponseForCustomer)
            .ToList();
    }
}