namespace Pharmacy.Models.Domain;

public class ExpiryDateItems : BaseEntity
{
    public long PharmacyId { get; set; }
    public long ExpiryDateProductId { get; set; }
    public ExpiryDate ExpiryDate { get; set; } = null!;
    public long ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal TotalPurchasePrice { get; set; }
    public decimal TotalOrderPrice { get; set; }
}