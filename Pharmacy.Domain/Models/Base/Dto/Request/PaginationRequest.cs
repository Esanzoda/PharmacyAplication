namespace Pharmacy.Domain.Models.Base.Dto.Request;

public class PaginationRequest
{
    public int PageNumber { get; set; }
    public const int PageSize = 10;
}