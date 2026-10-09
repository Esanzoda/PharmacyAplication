using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Product.DTos.Request;
using Pharmacy.Domain.Models.Product.DTos.Response;
using Refit;

namespace Product.Endpoint.Clients.IProductApi;

public interface IProductForPharmacy
{
    [Post("/api/ProductForPharmacy/Add")]
    Task<ProductForPharmacyResponse> Add(long pharmacyId,
        [Body] ProductRequest request);


    [Put("/api/ProductForPharmacy/Update")]
    Task<ProductForPharmacyResponse> Update(long pharmacyId,
        long id,
        [Body] UpdateProductRequest request
    );
    
    [Delete("/api/ProductForPharmacy/DeleteById")]
    Task<string> DeleteById(long pharmacyId, long id);


    [Get("/api/ProductForPharmacy/GetById")]
    Task<ProductForPharmacyResponse> GetById(long pharmacyId, long id);


    [Get("/api/ProductForPharmacy/GetAll")]
    Task<List<ProductForPharmacyResponse>> GetAll(long pharmacyId,
        [Query] PaginationRequest paginationRequest);


    [Get("/api/ProductForPharmacy/GetByBarcode")]
    Task<ProductForPharmacyResponse> GetByBarcode(long pharmacyId, string barcode);


    [Get("/api/ProductForPharmacy/GetByName")]
    Task<List<ProductForPharmacyResponse>> GetByName(long pharmacyId, string name,
        [Query] PaginationRequest paginationRequest);


    [Get("/api/ProductForPharmacy/GetByCategory")]
    Task<List<ProductForPharmacyResponse>> GetByCategory(long pharmacyId,
        long categoryId,
        [Query] PaginationRequest paginationRequest);


    [Get("/api/ProductForPharmacy/GetOutOfStock")]
    Task<List<ProductForPharmacyResponse>> GetOutOfStock(long pharmacyId,
        [Query] PaginationRequest paginationRequest);

    [Get("/api/ProductForPharmacy/GetLowOfStock")]
    Task<List<ProductForPharmacyResponse>> GetLowOfStock(long pharmacyId,
        int minimumQuantity, [Query] PaginationRequest paginationRequest);


    [Get("/api/ProductForPharmacy/GetByPurchasePrice")]
    Task<List<ProductForPharmacyResponse>> GetByPurchasePrice(long pharmacyId,
        decimal price, [Query] PaginationRequest paginationRequest);


    [Get("/api/ProductForPharmacy/GetByOrderPrice")]
    Task<List<ProductForPharmacyResponse>> GetByOrderPrice(long pharmacyId,
        decimal price,
        [Query] PaginationRequest paginationRequest);


    [Get("/api/ProductForPharmacy/GetByCountry")]
    Task<List<ProductForPharmacyResponse>> GetByCountry(long pharmacyId,
        CountryEnum country,
        [Query] PaginationRequest paginationRequest);
}