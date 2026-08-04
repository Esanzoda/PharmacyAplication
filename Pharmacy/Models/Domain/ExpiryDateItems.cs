using Pharmacy.CQRS.Product.ProductModels;

namespace Pharmacy.Models.Domain;

public class ExpiryDateItems : BaseEntity
{
    public long PharmacyId { get; set; }
    public long ExpiryDateId { get; set; }
    public ExpiryDate ExpiryDate { get; set; } = null!;
    public long ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public decimal TotalPurchasePrice { get; set; }
    public decimal TotalSalePrice { get; set; }
}