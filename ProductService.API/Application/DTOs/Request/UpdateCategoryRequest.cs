using ProductService.Api.Domain.Enums;

namespace ProductService.Api.Application.DTOs.Request;

public record UpdateCategoryRequest
{
    public required string Name { get; init; }
    public string Description { get; init; } = string.Empty;
    public CategoryStatus CategoryStatus { get; init; }
}