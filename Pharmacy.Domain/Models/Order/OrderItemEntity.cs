using Pharmacy.Domain.Models.Base.Domain;

namespace Pharmacy.Domain.Models.Order;

public class OrderItemEntity : BaseEntity
{
    public long OrderEntityId { get; set; }
    public long ProductEntityId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItemBatch> OrderItemBatches { get; set; } = [];
}