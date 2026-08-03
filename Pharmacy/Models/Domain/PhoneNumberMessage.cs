namespace Pharmacy.Models.Domain;

public class PhoneNumberMessage
{
    public long PharmacyId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}