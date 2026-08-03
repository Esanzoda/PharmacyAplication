using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Product.ProductModels.DTos.Request;

public record ProductRequest
{
    public required string Name { get; init; }
    public ProductType ProductType { get; init; }
    public long CategoryId { get; init; }
    public string Description { get; init; } = string.Empty;
    public CountryEnum Country { get; init; }
    public decimal PurchasePrice { get; init; }
    public decimal SalePrice { get; init; }
    public required string Barcode { get; init; }
    public int Stock { get; init; }
    public DateTime ExpiryDate { get; init; }
}