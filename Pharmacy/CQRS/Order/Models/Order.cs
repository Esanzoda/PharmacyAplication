using Pharmacy.Models.Domain;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Order.Models;

public class Order : BaseEntity
{
    public long CustomerId { get; set; }
    public long PharmacyId { get; set; }
    public Deliver.Models.Deliver? Deliver { get; set; }
    public OrderType OrderType { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public string Address { get; set; } = string.Empty;
    public DateTime? PicKupTime { get; set; }
    public Customer.Models.Customer Customer { get; set; } = null!;
    public decimal DeliveryFee { get; set; }
    public decimal TotalAmount { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new();
}