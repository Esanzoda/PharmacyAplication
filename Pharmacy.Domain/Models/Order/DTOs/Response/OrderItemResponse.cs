namespace Pharmacy.Domain.Models.Order.DTOs.Response;

public record OrderItemResponse
{
    public long Id { get; init; }
    public long ProductEntityId { get; init; }
    public int Quantity { get; init; }
    public decimal Price { get; init; }
    public decimal TotalPrice { get; init; }
}