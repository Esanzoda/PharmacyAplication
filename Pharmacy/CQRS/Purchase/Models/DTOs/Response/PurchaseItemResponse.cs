using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Purchase.Models.DTOs.Response;

public record PurchaseItemResponse
{
    public long Id { get; init; }
    public long PharmacyId { get; set; }
    public long PurchaseId { get; set; }
    public long ProductId { get; init; }
    public decimal PurchasePrice { get; init; }
    public CountryEnum Country { get; set; }
    public int Quantity { get; init; }
    public string? Barcode { get; init; }
    public decimal TotalPrice { get; init; }
}