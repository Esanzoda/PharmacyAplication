using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Domain.Models.Base.Domain.Enum;

namespace Pharmacy.Domain.Models.PurchaseEntity;

public class PurchaseItem : BaseEntity
{
    public long PharmacyId { get; set; }
    public long PurchaseEntityId { get; set; }
    public PurchaseEntity.Purchase PurchaseEntity { get; set; } = null!;
    public long ProductEntityId { get; set; }
    public decimal PurchasePrice { get; set; }
    public CountryEnum Country { get; set; }
    public int Quantity { get; set; }
    public string Barcode { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public DateOnly ProductionDate { get; set; }
    public DateOnly ExpiryDate { get; set; }
}