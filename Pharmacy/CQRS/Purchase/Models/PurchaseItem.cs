using Pharmacy.Models.Domain;

namespace Pharmacy.CQRS.Purchase.Models;

public class PurchaseItem : BaseEntity
{
    public long PharmacyId { get; set; }
    public long PurchaseId { get; set; }
    public long ProductId { get; set; }
    public Product.ProductModels.Product Product { get; set; }= null!;
    public decimal PurchasePrice { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Barcode { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public DateTime ExpiryDate { get; set; }
}