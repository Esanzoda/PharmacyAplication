using ProductServiceDomain.Enums;

namespace ProductService.Application.DTOs.Response;

public class ProductBatchResponse
{
    public long Id { get; init; }
    public int Quantity { get; init; }
    public CountryEnum Country { get; init; }
    public DateOnly ProductionDate { get; init; }
    public DateOnly ExpiryDate { get; init; }
    public decimal PurchasePrice { get; init; }
}