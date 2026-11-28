using ProductService.Contract.Enums;

namespace ProductService.Contract.Response;

public record CategoryResponse
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public string Description { get; init; } = string.Empty;
    public CategoryStatus CategoryStatus { get; set; }
}