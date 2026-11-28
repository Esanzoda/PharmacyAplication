using ProductService.Contract.Enums;

namespace ProductService.Contract.Request;

public record UpdateCategoryRequest
{
    public required string Name { get; init; }
    public string Description { get; init; } = string.Empty;
    public CategoryStatus CategoryStatus { get; init; }
}