using Pharmacy.Domain.Models.Base.Domain.Enum;

namespace Pharmacy.Domain.Models.Product.DTos.Response;

public class ProductForPharmacyResponse
{
    public long Id { get; init; }
    public long PharmacyId { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public long CategoryId { get; init; }
    public int Stock { get; init; }
    public decimal SalePrice { get; init; }
    public required string Barcode { get; init; }
    public ProductType ProductType { get; init; }
    public List<ProductBatchResponse> ProductBatchResponses { get; set; } = [];
}