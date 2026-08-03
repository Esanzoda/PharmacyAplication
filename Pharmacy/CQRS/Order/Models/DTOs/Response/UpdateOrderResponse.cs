using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Order.Models.DTOs.Response;

public record UpdateOrderResponse
{
    public long CustomerId { get; set; }
    public OrderStatus OrderStatus { get; set; }
}