using Pharmacy.CQRS.Order.Models;
using Pharmacy.Models.Domain;

namespace Pharmacy.CQRS.Deliver.Models;

public class DeliverEntity : User
{
    public decimal Shot { get; set; }
    public List<OrderEntity> Orders { get; set; } = [];
}