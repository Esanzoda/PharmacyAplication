using Pharmacy.Domain.Models.Base.Domain.Enum;

namespace Pharmacy.Domain.Models.Employee.DTOs.Request;

public record EmployeeRequest
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Password { get; init; }
    public required string Address { get; init; }
    public decimal Salary { get; init; }
    public Position Position { get; init; }
}