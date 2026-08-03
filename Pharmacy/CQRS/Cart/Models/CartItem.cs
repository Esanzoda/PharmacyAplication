using Pharmacy.Models.Domain;

namespace Pharmacy.CQRS.Cart.Models;

public class CartItem : BaseEntity
{
    public long CustomerId { get; set; }
    public long ProductId { get; set; }
    public Product.ProductModels.Product Product { get; set; } = null!;
    public Models.Cart Cart { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}