using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Purchase.Models.DTOs.Request;

public record PurchaseItemRequest
{
    public long ProductEntityId { get; init; }
    public int Quantity { get; init; }
    public decimal PurchasePrice { get; init; }
    public DateOnly ExpiryDate { get; init; }
    public CountryEnum Country { get; init; }
    public DateOnly ProductionDate { get; init; }
}