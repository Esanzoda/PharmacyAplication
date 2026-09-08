using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Deliver;
using Pharmacy.Domain.Models.Deliver.DTOs.Request;
using Pharmacy.Domain.Models.Deliver.DTOs.Response;
using Pharmacy.Domain.Models.Order;
using Pharmacy.Domain.Models.Order.DTOs.Response;

namespace Pharmacy.CQRS.Deliver.Mapper;

public static class DeliverMappers
{
    public static DeliverEntity ToDeliver(DeliverRequest request)
    {
        return new DeliverEntity
        {
            Name = request.Name,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Address = request.Address,
            Role = Role.Deliver,
            Shot = 0
        };
    }

    public static void ToDeliver(DeliverEntity deliver, UpdateDeliverRequest request)
    {
        deliver.Name = request.Name;
        deliver.PhoneNumber = request.PhoneNumber;
        deliver.Email = request.Email;
        deliver.Address = request.Address;
    }

    public static DeliverResponse ToDeliverResponse(DeliverEntity deliver)
    {
        return new DeliverResponse
        {
            Id = deliver.Id,
            Name = deliver.Name,
            Address = deliver.Address,
            PhoneNumber = deliver.PhoneNumber,
            Email = deliver.Email,
            Shot = deliver.Shot
        };
    }

    public static List<DeliverResponse> ToListDeliverResponse(List<DeliverEntity> delivers)
    {
        return delivers
            .Select(ToDeliverResponse)
            .ToList();
    }


    public static OrderResponseForDeliver ToReserveOrderForDeliver(OrderEntity order)
    {
        return new OrderResponseForDeliver
        {
            Id = order.Id,
            CustomerId = order.CustomerEntityId,
            PharmacyId = order.PharmacyId,
            OrderType = order.OrderType,
            Address = order.Address,
            OrderStatus = order.OrderStatus,
            DeliveryFee = order.DeliveryFee,
            TotalAmount = order.TotalAmount,
            CreatedAt = order.CreatedAt,
            DeliverEntity = order.Deliver!
        };
    }

    public static List<OrderResponseForDeliver> ToListReserveOrdersForDeliver(List<OrderEntity> orders)
    {
        return orders
            .Select(ToReserveOrderForDeliver)
            .ToList();
    }
}