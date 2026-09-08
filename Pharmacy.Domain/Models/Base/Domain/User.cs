using Pharmacy.Domain.Models.Base.Domain.Enum;

namespace Pharmacy.Domain.Models.Base.Domain;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
}