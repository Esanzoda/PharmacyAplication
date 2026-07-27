namespace Pharmacy.Models.Dto.Request;

public record PurchaseRequest
{
    public List<PurchaseItemRequest> PurchaseItems { get; set; }
}