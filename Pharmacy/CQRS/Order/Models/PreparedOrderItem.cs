using Pharmacy.CQRS.Product.ProductModels;

namespace Pharmacy.CQRS.Order.Models;

public class PreparedOrderItem
{
    public ProductEntity ProductEntity { get; set; } = null!;
    public int Quantity { get; set; }
}