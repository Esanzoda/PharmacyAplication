namespace Pharmacy.Domain.Models.PurchaseEntity.DTOs.Request;

public record PurchaseRequest
{
    public required List<PurchaseItemRequest> PurchaseItems { get; set; }
}