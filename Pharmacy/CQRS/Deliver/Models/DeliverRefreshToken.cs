namespace Pharmacy.CQRS.Deliver.Models;

public class DeliverRefreshToken
{
    public long DeliverEntityId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DeliverEntity DeliverEntity { get; set; } = null!;
}