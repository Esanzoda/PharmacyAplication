using Pharmacy.Domain.Models.Base.Domain.Enum;

namespace Pharmacy.Domain.Models.Base.Domain;

public class RefreshToken : BaseEntity
{
    public long UserId { get; set; }
    public Role Role { get; set; }
    public required string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}