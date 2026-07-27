namespace Pharmacy.Models.Domain;

public class ExpiryDate : BaseEntity
{
    
    public decimal TotalOrderPrice { get; set; }
    public decimal TotalPurchasePrice { get; set; }

    public List<ExpiryDateItems> ExpiryDateItemsList { get; set; } = new();
}