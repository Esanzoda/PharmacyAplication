using Pharmacy.CQRS.Product.ProductModels;
using Pharmacy.Models.Domain;

namespace Pharmacy.CQRS.Order.Models;

public class OrderItemEntity : BaseEntity
{
    public long ProductEntityId { get; set; }
    public ProductEntity ProductEntity { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}