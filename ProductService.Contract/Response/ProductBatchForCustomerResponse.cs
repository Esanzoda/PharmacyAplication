using ProductService.Contract.Enums;

namespace ProductService.Contract.Response;

public class ProductBatchForCustomerResponse
{
    public int Quantity { get; init; }
    public CountryEnum Country { get; init; }
    public DateOnly ProductionDate { get; init; }
    public DateOnly ExpiryDate { get; init; }
}