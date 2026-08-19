namespace Pharmacy.CQRS.Employee.Models;

public class EmployeeRefreshToken
{
    public long EmployeeId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public EmployeeEntity Employee { get; set; } = null!;
}