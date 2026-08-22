using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Models.Domain;

public class RefreshToken : BaseEntity
{
    public long UserId { get; set; }
    public Role Role { get; set; }
    public required string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}