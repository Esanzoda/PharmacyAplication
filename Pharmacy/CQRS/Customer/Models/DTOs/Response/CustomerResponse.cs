namespace Pharmacy.CQRS.Customer.Models.DTOs.Response;

public record CustomerResponse
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Address { get; init; }
    public required string PhoneNumber { get; init; }
}