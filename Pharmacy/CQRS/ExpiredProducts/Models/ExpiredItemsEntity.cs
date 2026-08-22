using Pharmacy.CQRS.Product.ProductModels;
using Pharmacy.Models.Domain;

namespace Pharmacy.CQRS.ExpiredProducts.Models;

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