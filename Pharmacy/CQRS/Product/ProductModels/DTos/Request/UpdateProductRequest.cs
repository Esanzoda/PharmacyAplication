using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Product.ProductModels.DTos.Request;

public class UpdateProductRequest
{
    public required string Name { get; init; }
    public ProductType ProductType { get; init; }
    public long CategoryId { get; init; }
    public string Description { get; init; } = string.Empty;
    public CountryEnum Country { get; init; }
}