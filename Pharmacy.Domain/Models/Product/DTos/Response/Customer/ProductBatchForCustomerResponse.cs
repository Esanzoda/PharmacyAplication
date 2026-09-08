using Pharmacy.Domain.Models.Base.Domain.Enum;

namespace Pharmacy.Domain.Models.Product.DTos.Response.Customer;

public class ProductBatchForCustomerResponse
{
    public int Quantity { get; init; }
    public CountryEnum Country { get; init; }
    public DateOnly ProductionDate { get; init; }
    public DateOnly ExpiryDate { get; init; }
}