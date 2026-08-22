using Pharmacy.CQRS.Customer.Models;
using Pharmacy.CQRS.Deliver.Models;
using Pharmacy.Models.Domain;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Order.Models;

public class OrderEntity : BaseEntity
{
    public long CustomerEntityId { get; set; }
    public long PharmacyId { get; set; }
    public DeliverEntity? Deliver { get; set; }
    public OrderType OrderType { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public string Address { get; set; } = string.Empty;
    public DateTime? PicKupTime { get; set; }
    public CustomerEntity CustomerEntity { get; set; } = null!;
    public decimal DeliveryFee { get; set; }
    public decimal TotalAmount { get; set; }

    public List<OrderItemEntity> OrderItems { get; set; } = [];
}