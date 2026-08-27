using Pharmacy.Models.Domain;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Employee.Models;

public class EmployeeEntity : User
{
    public long PharmacyId { get; set; }
    public decimal Salary { get; set; }
    public Position Position { get; set; }
}