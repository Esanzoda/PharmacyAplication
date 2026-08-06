using Pharmacy.Models.Domain;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Product.ProductModels;

public class ProductBatch : BaseEntity
{
    public long PharmacyId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
    public long PurchaseItemId { get; set; }
    public required Product Product { get; set; }
    public decimal PurchasePrice { get; set; }
    public CountryEnum Country { get; set; }
    public DateTime ProductionDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsActive { get; set; }
}