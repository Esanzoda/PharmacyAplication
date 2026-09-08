using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Domain.Models.Product;

namespace Pharmacy.Domain.Models.Expired;

public class ExpiredItemsEntity : BaseEntity
{
    public long PharmacyId { get; set; }
    public long ExpiryDateEntityId { get; set; }
    public ExpiredProductsEntity ExpiredProductsEntity { get; set; } = null!;
    public long ProductBatchId { get; set; }
    public ProductBatch ProductBatch { get; set; } = null!;
    public decimal TotalPurchasePrice { get; set; }
    public decimal TotalSalePrice { get; set; }
}