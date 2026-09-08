using Pharmacy.Domain.Models.Product;

namespace Pharmacy.Domain.Models.Order;

public class PreparedOrderItem
{
    public ProductEntity ProductEntity { get; set; } = null!;
    public int Quantity { get; set; }
}