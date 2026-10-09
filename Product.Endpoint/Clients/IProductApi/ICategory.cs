using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Category.DTOs.Request;
using Pharmacy.Domain.Models.Category.DTOs.Response;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using Refit;

namespace Product.Endpoint.Clients.IProductApi;

public interface ICategory
{
    [Post("/api/Category/Add")]
    Task<CategoryResponse> Add(
        [Body] CreateCategoryRequest request);


    [Put("/api/Category/")]
    Task<CategoryResponse> Update(long id, [Body] UpdateCategoryRequest request);


    [Get("/api/Category/Update")]
    Task<CategoryResponse> GetById(long id,
        CancellationToken cancellationToken = default);


    [Get("/api/Category/GetAll")]
    Task<List<CategoryResponse>> GetAll([Query] PaginationRequest paginationRequest);


    [Delete("/api/Category/DeleteById")]
    Task<string> DeleteById(long id);


    [Get("/api/Category/GetProducts")]
    Task<List<ProductForCustomerResponse>> GetProducts(int categoryId,
        [Query] PaginationRequest paginationRequest);


    [Get("/api/Category/GetByName")]
    Task<List<CategoryResponse>> GetByName(string name);


    [Get("/api/Category/GetByStatus")]
    Task<List<CategoryResponse>> GetByStatus(CategoryStatus categoryStatus,
        [Query] PaginationRequest paginationRequest);
}