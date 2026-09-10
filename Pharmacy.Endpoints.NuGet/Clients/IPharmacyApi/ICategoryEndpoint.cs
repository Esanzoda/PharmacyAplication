using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Category.DTOs.Request;
using Pharmacy.Domain.Models.Category.DTOs.Response;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using Refit;

namespace Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

public interface ICategoryEndpoint
{
    [Post("api/Category/Add")]
    Task<CategoryResponse> Add(
        [Body] CreateCategoryRequest request);
    
    [Post("api/Category/Update")]
    Task<CategoryResponse> Update(
        long id, 
        [Body] UpdateCategoryRequest request);

    [Get("api/Category/GetById")]
    Task<CategoryResponse> GetById(
        long id,
        CancellationToken cancellationToken = default);
    
    [Get("api/Category/GetAll")]
    Task<List<CategoryResponse>> GetAll(
        int page, 
        int pageSize);

    [Delete("api/Category/DeleteById")]
    Task DeleteById(
        long id);


    [Get("api/Category/GetProducts")]
    Task<List<ProductForCustomerResponse>> GetProducts(
        int categoryId, 
        int page,
        int pageSize);


    [Get("api/Category/GetByName")]
    Task<CategoryResponse> GetByName(
        string name);


    [Get("api/Category/GetByStatus")]
    Task<List<CategoryResponse>> GetByStatus(
        CategoryStatus categoryStatus, 
        int pageNumber,
        int pageSize);


}