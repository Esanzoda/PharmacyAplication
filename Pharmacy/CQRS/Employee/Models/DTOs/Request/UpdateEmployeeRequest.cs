namespace Pharmacy.CQRS.Employee.Models.DTOs.Request;

public class UpdateEmployeeRequest
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Address { get; init; }
}