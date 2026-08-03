using Pharmacy.Models.Domain;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Customer.Models;

public class Customer : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.Customer;
    public Cart.Models.Cart Cart { get; set; } = null!;
}