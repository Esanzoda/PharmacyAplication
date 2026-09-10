using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using Refit;

namespace Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

public interface IProductEndpoint
{
    [Get("/api/Product/GetAll")]
     Task<List<ProductForCustomerResponse>> GetAll(
         int pageNumber, 
         int pageSize);

    [Get("/api/Product/GetByNameAsync")]
     Task<List<ProductForCustomerResponse>> GetByNameAsync(
         string name, 
         int page,
        int pageSize);

    [Get("/api/Product/GetByCategoryIdAsync")]
    Task<List<ProductForCustomerResponse>> GetByCategoryIdAsync(
        long categoryId,
        int page,
        int pageSize);

    [Get("/api/Product/GetBySalePriceAsync")]
    Task<List<ProductForCustomerResponse>> GetBySalePriceAsync(
        decimal price, 
        int page,
        int pageSize);

    [Get("/api/Product/GetByCountryAsync")]
    Task<List<ProductForCustomerResponse>> GetByCountryAsync(
        CountryEnum country,
        int page,
        int pageSize);

    [Get("/api/Product/GetById")]
     Task<ProductForCustomerResponse> GetById(
         long productId);

    [Get("/api/Product/SearchByProductNameFromPharmacy")]
    Task<List<ProductForCustomerResponse>> SearchByProductNameFromPharmacy(
        long pharmacyId,
        string name, 
        int pageNumber,
        int pageSize);
}