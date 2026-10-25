using Pharmacy.Domain.Models.Base.Domain;

namespace Pharmacy.Domain.Models.Order;

public class OrderItemBatch : BaseEntity
{
    public long OrderItemId { get; set; }
    public long ProductBatchId { get; set; }
    public int Quantity { get; set; }
}