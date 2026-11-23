namespace PharmacyService.Api.Application.DTOs.Request;

public record CreateOrderItemRequest
{
    public long ProductId { get; init; }
    public int Quantity { get; init; }
}