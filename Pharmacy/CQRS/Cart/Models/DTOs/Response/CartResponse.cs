namespace Pharmacy.CQRS.Cart.Models.DTOs.Response;

public record CartResponse
{
    public long Id { get; init; }
    public decimal TotalAmount { get; init; }
    public List<CartItemResponse> CartItemResponse { get; init; } = [];
}

public record CartItemResponse
{
    public long ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal Price { get; init; }
    public decimal TotalPrice { get; init; }
}