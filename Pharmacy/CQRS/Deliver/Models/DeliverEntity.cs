using Pharmacy.CQRS.Order.Models;
using Pharmacy.Models.Domain;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Deliver.Models;

public class DeliverEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal Shot { get; set; }
    public Role Role { get; set; }
    public  string PasswordHash { get; set; } = string.Empty;
    public List<OrderEntity> Orders { get; set; } = [];
}