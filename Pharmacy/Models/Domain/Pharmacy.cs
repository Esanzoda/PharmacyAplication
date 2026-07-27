namespace Pharmacy.Models.Domain;

public class Pharmacy : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public TimeOnly OpeningTime { get; set; }
    public TimeOnly ClosingTime { get; set; }

}