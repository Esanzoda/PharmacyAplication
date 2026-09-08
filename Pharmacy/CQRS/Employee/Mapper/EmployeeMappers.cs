using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Employee;
using Pharmacy.Domain.Models.Employee.DTOs.Request;
using Pharmacy.Domain.Models.Employee.DTOs.Response;

namespace Pharmacy.CQRS.Employee.Mapper;

public static class EmployeeMappers
{
    public static EmployeeEntity ToEmployee(EmployeeRequest request)
    {
        return new EmployeeEntity
        {
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            Salary = request.Salary,
            Position = request.Position,
            Role = Role.Employee
        };
    }

    public static void ToEmployee(EmployeeEntity employee, UpdateEmployeeRequest request)
    {
        employee.Name = request.Name;
        employee.Email = request.Email;
        employee.PhoneNumber = request.PhoneNumber;
        employee.Address = request.Address;
    }

    public static EmployeeResponse ToEmployeeResponse(EmployeeEntity employee)
    {
        return new EmployeeResponse
        {
            Id = employee.Id,
            PharmacyId = employee.PharmacyId,
            Name = employee.Name,
            Address = employee.Address,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            Salary = employee.Salary,
            Position = employee.Position
        };
    }

    public static List<EmployeeResponse> ToListEmployeeResponse(List<EmployeeEntity> employees)
    {
        return employees
            .Select(ToEmployeeResponse)
            .ToList();
    }
}