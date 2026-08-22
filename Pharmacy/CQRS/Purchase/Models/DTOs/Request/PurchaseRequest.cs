namespace Pharmacy.CQRS.Purchase.Models.DTOs.Request;

public record PurchaseRequest
{
    public required List<PurchaseItemRequest> PurchaseItems { get; set; }
}