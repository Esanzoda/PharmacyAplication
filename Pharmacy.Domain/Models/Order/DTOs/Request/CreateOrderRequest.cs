using Pharmacy.Domain.Models.Base.Domain.Enum;

namespace Pharmacy.Domain.Models.Order.DTOs.Request;

public record CreateOrderRequest
{
    public OrderType OrderType { get; init; }
    public DateTime? PicKupTime { get; init; }
    public required List<CreateOrderItemRequest> OrderItemRequest { get; init; }
}