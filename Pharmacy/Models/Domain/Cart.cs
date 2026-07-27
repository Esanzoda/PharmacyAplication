namespace Pharmacy.Models.Domain;

public class Cart : BaseEntity
{
    public long CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public decimal TotalAmount { get; set; }

    public List<CartItem> CartItems { get; set; } = new();
}