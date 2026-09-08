using Pharmacy.Domain.Models.Base.Domain.Enum;

namespace Pharmacy.Domain.Models.Product.DTos.Response.Customer;

public record ProductForCustomerResponse
{
    public long Id { get; init; }
    public long PharmacyId { get; init; }
    public required string Name { get; init; }
    public string Description { get; init; } = string.Empty;
    public long CategoryEntityId { get; init; }
    public int Stock { get; init; }
    public decimal SalePrice { get; init; }
    public required string Barcode { get; init; }
    public ProductType ProductType { get; init; }
    public List<ProductBatchForCustomerResponse> ProductBatchForCustomerResponses { get; set; } = [];
}