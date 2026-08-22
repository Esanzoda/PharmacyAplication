using Pharmacy.Models.Domain;

namespace Pharmacy.CQRS.ExpiredProducts.Models;

public class ExpiredProductsEntity : BaseEntity
{
    public decimal TotalSalePrice { get; set; }
    public decimal TotalPurchasePrice { get; set; }
    public long PharmacyId { get; set; }
    public List<ExpiredItemsEntity> ExpiryDateItemsList { get; set; } = [];
}