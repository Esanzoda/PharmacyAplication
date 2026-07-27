using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Models.Domain;

public class Customer : BaseEntity
{
    public string Name { get; set; }= string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.Customer;
    public Cart Cart { get; set; }= null!;
 
    
}