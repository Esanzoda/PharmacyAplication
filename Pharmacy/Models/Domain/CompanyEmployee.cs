using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Models.Domain;

public class CompanyEmployee : BaseEntity
{
    public required string Name { get; set; }
    public Role Role { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
}