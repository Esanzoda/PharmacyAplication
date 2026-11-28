namespace PharmacyService.Api.Application.DTOs.Request;

public record PaginationRequest
{
    public int PageNumber { get; set; }
    public const int PageSize = 10;
}