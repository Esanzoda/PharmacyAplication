using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Customer;
using Pharmacy.Domain.Models.Deliver;

namespace Pharmacy.Domain.Models.Order;

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