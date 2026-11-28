namespace PharmacyService.Contract.Response;

public record PurchaseResponse
{
    public long Id { get; init; }
    public decimal TotalAmount { get; init; }
    public DateTime CreatedAt { get; init; }
    public long SupplierId { get; set; }
    public List<PurchaseItemResponse> PurchaseItems { get; set; } = [];
}