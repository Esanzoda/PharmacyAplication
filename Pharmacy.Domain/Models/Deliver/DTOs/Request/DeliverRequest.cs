namespace Pharmacy.Domain.Models.Deliver.DTOs.Request;

public record DeliverRequest
{
    public required string Name { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Email { get; init; }
    public required string Address { get; init; }
    public required string Password { get; init; }
}