namespace ProductService.Api.Application.DTOs.Request;

public record CreateCategoryRequest
{
    public required string Name { get; init; }
    public string Description { get; init; } = string.Empty;
}