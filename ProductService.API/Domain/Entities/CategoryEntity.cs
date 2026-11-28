using ProductService.Api.Domain.Enums;

namespace ProductService.Api.Domain.Entities;

public class CategoryEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public CategoryStatus CategoryStatus { get; set; }
    public List<ProductEntity> Products { get; set; } = [];
}