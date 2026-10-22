namespace Pharmacy.Domain.Models.PurchaseEntity.DTOs.Request;

public record PurchaseRequest
{
    public long SupplierId { get; set; }
    public required List<PurchaseItemRequest> PurchaseItems { get; set; }
}