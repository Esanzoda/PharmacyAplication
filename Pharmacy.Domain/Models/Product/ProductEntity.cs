using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Category;

namespace Pharmacy.Domain.Models.Product;

public class ProductEntity : BaseEntity
{
    public long PharmacyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Stock { get; set; }
    public string Barcode { get; set; } = string.Empty;
    public decimal SalePrice { get; set; }
    public ProductType ProductType { get; set; }
    public long CategoryEntityId { get; set; }
    public CategoryEntity CategoryEntity { get; set; } = null!;
    public List<ProductBatch> ProductBatches { get; set; } = [];
}