namespace Pharmacy.CQRS.Purchase.Models.DTOs.Response;

public class PurchaseResponse
{
    public long Id { get; init; }
    public decimal TotalAmount { get; init; }
    public DateTime CreatedAt { get; init; }
    public List<PurchaseItemResponse> PurchaseItems { get; set; }
}