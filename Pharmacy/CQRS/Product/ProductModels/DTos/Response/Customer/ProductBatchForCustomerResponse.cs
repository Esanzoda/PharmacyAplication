using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Product.ProductModels.DTos.Response.Customer;

public class ProductBatchForCustomerResponse
{
    public int Quantity { get; set; }
    public CountryEnum Country { get; set; }
    public DateTime ProductionDate { get; set; }
    public DateTime ExpiryDate { get; set; }
}