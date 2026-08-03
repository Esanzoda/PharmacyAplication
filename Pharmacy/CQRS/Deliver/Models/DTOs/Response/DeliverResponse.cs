namespace Pharmacy.CQRS.Deliver.Models.DTOs.Response;

public record DeliverResponse
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    public required string Email { get; set; }
    public decimal Shot { get; set; }
}