using Pharmacy.Domain.Models.Base.Domain.Enum;

namespace Pharmacy.Domain.Models.PurchaseEntity.DTOs.Request;

public record PurchaseItemRequest
{
    public long ProductEntityId { get; init; }
    public int Quantity { get; init; }
    public decimal PurchasePrice { get; init; }
    public DateOnly ExpiryDate { get; init; }
    public CountryEnum Country { get; init; }
    public DateOnly ProductionDate { get; init; }
}