using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Domain.Models.Order;

namespace Pharmacy.Domain.Models.Deliver;

public class DeliverEntity : User
{
    public decimal Shot { get; set; }
    public List<OrderEntity> Orders { get; set; } = [];
}