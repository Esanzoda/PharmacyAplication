namespace Pharmacy.Domain.Models.Deliver.DTOs.Response;

public class DeliverResponse
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public required string Address { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Email { get; init; }
    public decimal Shot { get; init; }
}