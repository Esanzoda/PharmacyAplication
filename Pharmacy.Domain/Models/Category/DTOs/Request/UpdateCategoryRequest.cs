using Pharmacy.Domain.Models.Base.Domain.Enum;

namespace Pharmacy.Domain.Models.Category.DTOs.Request;

public record UpdateCategoryRequest
{
    public required string Name { get; init; }
    public string Description { get; init; } = string.Empty;
    public CategoryStatus CategoryStatus { get; init; }
}