namespace Pharmacy.CQRS.Deliver.Models.DTOs.Response;

public class DeliverResponseForOrder
{
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string PhoneNumber { get; set; }
}