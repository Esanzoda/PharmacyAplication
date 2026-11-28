using PharmacyService.Contract.Enums;

namespace PharmacyService.Contract.Entities;

public class CategoryEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public CategoryStatus CategoryStatus { get; set; }
    public List<ProductEntity> Products { get; set; } = [];
}