namespace Pharmacy.CQRS.Customer.Models.DTOs.Request;

public record UpdateCustomerRequest
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Address { get; init; }
}