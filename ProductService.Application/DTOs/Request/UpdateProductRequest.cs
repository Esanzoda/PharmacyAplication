using ProductServiceDomain.Enums;

namespace ProductService.Application.DTOs.Request;

public class UpdateProductRequest
{
    public required string Name { get; init; }
    public ProductType ProductType { get; init; }
    public long CategoryId { get; init; }
    public string Description { get; init; } = string.Empty;
}