namespace Pharmacy.CQRS.Category.Models.DTOs.Response;

public record CategoryResponse
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public string Description { get; init; } = string.Empty;
}