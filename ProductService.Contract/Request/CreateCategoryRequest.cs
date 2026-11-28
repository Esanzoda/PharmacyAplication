namespace ProductService.Contract.Request;

public record CreateCategoryRequest
{
    public required string Name { get; init; }
    public string Description { get; init; } = string.Empty;
}