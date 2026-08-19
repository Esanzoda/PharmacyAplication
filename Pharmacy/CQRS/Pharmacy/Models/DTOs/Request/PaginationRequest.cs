namespace Pharmacy.CQRS.Pharmacy.Models.DTOs.Request;

public class PaginationRequest
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}