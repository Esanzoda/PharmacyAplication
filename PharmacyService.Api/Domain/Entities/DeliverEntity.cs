namespace PharmacyService.Api.Domain.Entities;

public class DeliverEntity : User
{
    public decimal Shot { get; set; }
    public List<OrderEntity> Orders { get; set; } = [];
}