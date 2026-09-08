using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Domain.Models.Product;

namespace Pharmacy.Domain.Models.Order;

public class OrderItemEntity : BaseEntity
{
    public long ProductEntityId { get; set; }
    public ProductEntity ProductEntity { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}