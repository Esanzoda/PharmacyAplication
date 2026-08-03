using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Product.ProductModels.DTos.Response;

public record ProductResponse
{
    public long PharmacyId { get; init; }
    public long Id { get; init; }
    public required string Name { get; init; }
    public string Description { get; init; } = string.Empty;
    public long CategoryId { get; init; }
    public CountryEnum Country { get; init; }
    public int Stock { get; init; }
    public decimal Price { get; init; }
    public DateTime ExpiryDate { get; init; }
    public required string Barcode { get; init; }
    public ProductType ProductType { get; init; }
}