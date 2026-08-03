using Pharmacy.Models.Domain;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Product.ProductModels;

public class Product : BaseEntity
{
    public long PharmacyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Stock { get; set; }
    public string Barcode { get; set; } = string.Empty;
    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }
    public ProductType ProductType { get; set; }
    public long CategoryId { get; set; }
    public Category.Models.Category Category { get; set; }= null!;
    public DateTime ExpiryDate { get; set; }
    public CountryEnum Country { get; set; }
}