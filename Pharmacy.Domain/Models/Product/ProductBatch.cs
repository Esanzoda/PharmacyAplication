using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.PurchaseEntity;

namespace Pharmacy.Domain.Models.Product;

public class ProductBatch : BaseEntity
{
    public long PharmacyId { get; set; }
    public long ProductEntityId { get; set; }
    public required string Name { get; set; }
    public required ProductEntity ProductEntity { get; set; }
    public CountryEnum Country { get; set; }
    public int Quantity { get; set; }
    public long PurchaseItemId { get; set; }
    public PurchaseItem PurchaseItem { get; set; } = null!;
    public DateOnly ProductionDate { get; set; }
    public DateOnly ExpiryDate { get; set; }
    public decimal PurchasePrice { get; set; }
    public bool IsActive { get; set; }
    public decimal TotalPurchasePrice { get; set; }
}