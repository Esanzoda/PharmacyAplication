using Pharmacy.Models.Domain;

namespace Pharmacy.CQRS.Order.Models;

public class OrderItem : BaseEntity
{
   
    public long ProductId { get; set; }
    public Product.ProductModels.Product Product { get; set; }= null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}