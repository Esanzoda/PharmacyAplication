namespace Pharmacy.Models.Domain;

public class ExpiryDate : BaseEntity
{
    public decimal TotalSalePrice { get; set; }
    public decimal TotalPurchasePrice { get; set; }

    public List<ExpiryDateItems> ExpiryDateItemsList { get; set; } = new();
}