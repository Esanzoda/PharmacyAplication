using PharmacyService.Domain.Enums;

namespace PharmacyService.Domain.Entities;

public class CategoryEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public CategoryStatus CategoryStatus { get; set; }
    public List<ProductEntity> Products { get; set; } = [];
}