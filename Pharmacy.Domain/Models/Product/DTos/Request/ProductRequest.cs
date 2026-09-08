using Pharmacy.Domain.Models.Base.Domain.Enum;

namespace Pharmacy.Domain.Models.Product.DTos.Request;

public record ProductRequest
{
    public required string Name { get; init; }
    public ProductType ProductType { get; init; }
    public long CategoryId { get; init; }
    public string Description { get; init; } = string.Empty;
    public decimal SalePrice { get; init; }
    public required string Barcode { get; init; }
}