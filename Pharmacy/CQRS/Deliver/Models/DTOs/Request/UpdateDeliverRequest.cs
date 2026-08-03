namespace Pharmacy.CQRS.Deliver.Models.DTOs.Request;

public class UpdateDeliverRequest
{
    public required string Name { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Email { get; init; }
    public required string Address { get; init; }
}