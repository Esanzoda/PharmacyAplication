namespace Pharmacy.CQRS.Order.Models;

public class PreparedOrderItem
{
    public Product.ProductModels.Product Product { get; set; } = null!;
    public int Quantity { get; set; }
}