using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Purchase.Models.DTOs.Response;

public record PurchaseItemResponse
{
    public long Id { get; init; }
    public long PharmacyId { get; init; }
    public long PurchaseEntityId { get; init; }
    public long ProductEntityId { get; init; }
    public decimal PurchasePrice { get; init; }
    public CountryEnum Country { get; init; }
    public int Quantity { get; init; }
    public required string Barcode { get; init; }
    public decimal TotalPrice { get; init; }
}