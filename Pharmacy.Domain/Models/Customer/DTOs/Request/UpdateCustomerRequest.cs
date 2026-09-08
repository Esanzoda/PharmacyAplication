namespace Pharmacy.Domain.Models.Customer.DTOs.Request;

public record UpdateCustomerRequest
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
}