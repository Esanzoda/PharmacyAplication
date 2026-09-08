using Pharmacy.Domain.Models.Base.Domain.Enum;

namespace Pharmacy.Domain.Models.PurchaseEntity.DTOs.Response;

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