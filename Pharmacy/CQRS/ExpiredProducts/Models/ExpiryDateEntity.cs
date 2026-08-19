using Pharmacy.Models.Domain;

namespace Pharmacy.CQRS.ExpiredProducts.Models;

public class ExpiryDateEntity : BaseEntity
{
    public decimal TotalSalePrice { get; set; }
    public decimal TotalPurchasePrice { get; set; }
    public long PharmacyId { get; set; }
    public List<ExpiryDateItemsEntity> ExpiryDateItemsList { get; set; } = new();
}