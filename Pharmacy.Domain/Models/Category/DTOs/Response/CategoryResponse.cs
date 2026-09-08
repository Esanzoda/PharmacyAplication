using Pharmacy.Domain.Models.Base.Domain.Enum;

namespace Pharmacy.Domain.Models.Category.DTOs.Response;

public record CategoryResponse
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public string Description { get; init; } = string.Empty;
    public CategoryStatus CategoryStatus { get; set; }
}