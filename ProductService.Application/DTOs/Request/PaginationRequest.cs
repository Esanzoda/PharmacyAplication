namespace ProductService.Application.DTOs.Request;

public class PaginationRequest
{
    public int PageNumber { get; set; }
    public const int PageSize = 10;
}