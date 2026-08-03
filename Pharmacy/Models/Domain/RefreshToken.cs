using Pharmacy.CQRS.Customer.Models;

namespace Pharmacy.Models.Domain;

public class RefreshToken : BaseEntity
{
    public long CustomerId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public Customer Customer { get; set; } = null!;
}