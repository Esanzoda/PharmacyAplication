using ProductService.Contract.Enums;
using ProductService.Contract.Request;
using ProductService.Contract.Response;
using Refit;

namespace ProductService.ApiClient.Clients.IProductApi;

public interface IProductForCustomer
{
    [Get("/api/ProductForCustomer/GetAll")]
    Task<List<ProductForCustomerResponse>> GetAll(
        [Query] PaginationRequest paginationRequest);

    [Get("/api/ProductForCustomer/GetByName")]
    Task<List<ProductForCustomerResponse>> GetByName(
        string name,
        [Query] PaginationRequest paginationRequest);

    [Get("/api/ProductForCustomer/GetByCategoryId")]
    Task<List<ProductForCustomerResponse>> GetByCategoryId(
        long categoryId,
        [Query] PaginationRequest paginationRequest);

    [Get("/api/ProductForCustomer/GetBySalePrice")]
    Task<List<ProductForCustomerResponse>> GetBySalePrice(
        decimal price,
        [Query] PaginationRequest paginationRequest);

    [Get("/api/ProductForCustomer/GetByCountry")]
    Task<List<ProductForCustomerResponse>> GetByCountry(
        CountryEnum country,
        [Query] PaginationRequest paginationRequest);

    [Get("/api/ProductForCustomer/GetById")]
    Task<ProductForCustomerResponse> GetById(
        long productId);

    [Get("/api/ProductForCustomer/SearchByProductNameFromPharmacy")]
    Task<List<ProductForCustomerResponse>> SearchByProductNameFromPharmacy(
        long pharmacyId,
        string name,
        [Query] PaginationRequest paginationRequest);

    [Get("/api/ProductForCustomer/GetByIdWithRecommendation")]
    Task<List<ProductForCustomerResponse>> GetByIdWithRecommendation(
        long id,
        [Query] PaginationRequest paginationRequest);
}