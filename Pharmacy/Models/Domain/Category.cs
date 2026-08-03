using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Models.Domain;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public CategoryStatus CategoryStatus { get; set; }
    public List<Product> Products { get; set; } = new ();
}