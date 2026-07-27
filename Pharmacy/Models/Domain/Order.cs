using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Models.Domain;

public class Order : BaseEntity
{
    public long CustomerId { get; set; }
    public long PharmacyId { get; set; }
    public Deliver? Deliver { get; set; }
    public OrderType OrderType { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public string Address { get; set; } = string.Empty;
    public DateTime? GetTime { get; set; }
    public Customer Customer { get; set; } = null!;
    public decimal TotalAmount { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new();
}