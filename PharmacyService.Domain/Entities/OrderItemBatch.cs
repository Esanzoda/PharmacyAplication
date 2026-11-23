namespace PharmacyService.Domain.Entities;

public class OrderItemBatch : BaseEntity
{
    public long OrderItemEntityId { get; set; }
    public long ProductBatchId { get; set; }
    public int Quantity { get; set; }
}