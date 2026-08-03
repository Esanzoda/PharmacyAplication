namespace Pharmacy.CQRS.Purchase.Models.DTOs.Request;

public record PurchaseItemRequest
{
    public long ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal PurchasePrice { get; init; }
    public decimal Price { get; init; }
    public required string Barcode { get; init; }
    public DateTime ExpiryDate { get; init; }
}