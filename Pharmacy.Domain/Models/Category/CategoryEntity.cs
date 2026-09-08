using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Product;

namespace Pharmacy.Domain.Models.Category;

public class CategoryEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public CategoryStatus CategoryStatus { get; set; }
    public List<ProductEntity> Products { get; set; } = [];
}