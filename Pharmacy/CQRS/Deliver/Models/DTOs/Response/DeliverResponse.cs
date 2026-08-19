namespace Pharmacy.CQRS.Deliver.Models.DTOs.Response;

public class DeliverResponse
{
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
}