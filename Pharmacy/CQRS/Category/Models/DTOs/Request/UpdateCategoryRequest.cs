using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Category.Models.DTOs.Request;

public record UpdateCategoryRequest
{
    public required string Name { get; init; }
    public string Description { get; init; } = string.Empty;
    public CategoryStatus CategoryStatus { get; init; }
}