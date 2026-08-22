using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Order.Models.DTOs.Request;

public record CreateOrderRequest
{
    public OrderType OrderType { get; init; }
    public DateTime? PicKupTime { get; init; }
    public required List<CreateOrderItemRequest> OrderItemRequest { get; init; }
}