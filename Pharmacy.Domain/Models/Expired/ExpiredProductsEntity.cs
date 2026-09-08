using Pharmacy.Domain.Models.Base.Domain;

namespace Pharmacy.Domain.Models.Expired;

public class ExpiredProductsEntity : BaseEntity
{
    public decimal TotalSalePrice { get; set; }
    public decimal TotalPurchasePrice { get; set; }
    public long PharmacyId { get; set; }
    public List<ExpiredItemsEntity> ExpiryDateItemsList { get; set; } = [];
}