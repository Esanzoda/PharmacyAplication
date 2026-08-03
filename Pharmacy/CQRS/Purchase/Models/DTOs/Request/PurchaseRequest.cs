namespace Pharmacy.CQRS.Purchase.Models.DTOs.Request;

public record PurchaseRequest
{
    public List<PurchaseItemRequest> PurchaseItems { get; set; }
}