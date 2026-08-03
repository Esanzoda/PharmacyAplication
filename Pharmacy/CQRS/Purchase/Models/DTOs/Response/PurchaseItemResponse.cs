namespace Pharmacy.CQRS.Purchase.Models.DTOs.Response;

public record PurchaseItemResponse
{
    public long Id { get; init; }
    public long ProductId { get; init; }
    public decimal PurchasePrice { get; init; }
    public int Quantity { get; init; }
    public string? Barcode { get; init; }
    public decimal TotalPrice { get; init; }
}