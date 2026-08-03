using Pharmacy.Models.Domain;

namespace Pharmacy.CQRS.Cart.Models;

public class Cart : BaseEntity
{
    public long CustomerId { get; set; }
    public Customer.Models.Customer Customer { get; set; } = null!;
    public decimal TotalAmount { get; set; }

    public List<CartItem> CartItems { get; set; } = new();
}