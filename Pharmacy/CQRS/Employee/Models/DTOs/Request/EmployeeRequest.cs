using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Employee.Models.DTOs.Request;

public record EmployeeRequest
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Password { get; init; }
    public required string Address { get; init; }
    public decimal Salary { get; init; }
    public Role Role { get; init; }
}