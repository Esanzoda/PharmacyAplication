using PharmacyService.Domain.Enums;

namespace PharmacyService.Api.Application.DTOs.Request;

public record CreateOrderRequest
{
    public OrderType OrderType { get; init; }
    public DateTime? PicKupTime { get; init; }
    public required List<CreateOrderItemRequest> OrderItemRequest { get; init; }
}