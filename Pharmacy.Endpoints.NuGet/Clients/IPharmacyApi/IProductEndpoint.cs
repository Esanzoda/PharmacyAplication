using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using Refit;

namespace Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

public interface IProductEndpoint
{
    [Get("/api/ProductService/GetAll")]
     Task<List<ProductForCustomerResponse>> GetAll(
         int pageNumber, 
         int pageSize);

    [Get("/api/ProductService/GetByName")]
     Task<List<ProductForCustomerResponse>> GetByName(
         string name, 
         int page,
        int pageSize);

    [Get("/api/ProductService/GetByCategoryId")]
    Task<List<ProductForCustomerResponse>> GetByCategoryId(
        long categoryId,
        int page,
        int pageSize);

    [Get("/api/ProductService/GetBySalePrice")]
    Task<List<ProductForCustomerResponse>> GetBySalePrice(
        decimal price, 
        int page,
        int pageSize);

    [Get("/api/ProductService/GetByCountry")]
    Task<List<ProductForCustomerResponse>> GetByCountry(
        CountryEnum country,
        int page,
        int pageSize);

    [Get("/api/ProductService/GetById")]
     Task<ProductForCustomerResponse> GetById(
         long productId);

    [Get("/api/ProductService/SearchByProductNameFromPharmacy")]
    Task<List<ProductForCustomerResponse>> SearchByProductNameFromPharmacy(
        long pharmacyId,
        string name, 
        int pageNumber,
        int pageSize);
}