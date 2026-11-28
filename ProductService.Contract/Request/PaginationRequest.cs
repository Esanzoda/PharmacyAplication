namespace ProductService.Contract.Request;

public class PaginationRequest
{
    public int PageNumber { get; set; }
    public const int PageSize = 10;
}