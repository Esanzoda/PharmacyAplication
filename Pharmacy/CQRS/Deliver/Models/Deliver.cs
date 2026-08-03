using Pharmacy.Models.Domain;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Deliver.Models;

public class Deliver : BaseEntity
{
  
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal Shot { get; set; } = 0;
    public Role Role { get; set; } = Role.Deliver;
    public string PasswordHash { get; set; } = string.Empty;
    public List<Order.Models.Order> Orders { get; set; } = new();
}