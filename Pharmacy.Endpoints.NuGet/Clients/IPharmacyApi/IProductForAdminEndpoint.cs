using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Product.DTos.Request;
using Pharmacy.Domain.Models.Product.DTos.Response;
using Refit;

namespace Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

public interface IProductForAdminEndpoint
{
    [Post("/api/ProductControllerForAdmin/Add")]
    Task<ProductForPharmacyResponse> Add(
        [Body] ProductRequest request,
        long pharmacyId);

    [Put("/api/ProductControllerForAdmin/Update")]
    Task<ProductForPharmacyResponse> Update(
        long id, 
        [Body] UpdateProductRequest request,
        long pharmacyId);

    [Delete("/api/ProductControllerForAdmin/DeleteById")]
    Task<string> DeleteById(
        long id, 
        long pharmacyId);

    [Get("/api/ProductControllerForAdmin/GetById")]
   Task<ProductForPharmacyResponse> GetById(
       long id, 
       long pharmacyId);

    [Get("/api/ProductControllerForAdmin/GetAll")]
   Task<List<ProductForPharmacyResponse>> GetAll(
       long pharmacyId,
       int pageNumber,
        int pageSize);

    [Get("/api/ProductControllerForAdmin/GetByBarcode")]
     Task<ProductForPharmacyResponse> GetByBarcode(
         long pharmacyId, 
         string barcode);

    [Get("/api/ProductControllerForAdmin/GetByName")]
    Task<List<ProductForPharmacyResponse>> GetByName(
        long pharmacyId, 
        string name,
        int page,
        int pageSize);

    [Get("/api/ProductControllerForAdmin/GetByCategory")]
     Task<List<ProductForPharmacyResponse>> GetByCategory(
         long pharmacyId, 
         long categoryId,
        int pageNumber, 
         int pageSize);

    [Get("/api/ProductControllerForAdmin/GetOutOfStock")]
     Task<List<ProductForPharmacyResponse>> GetOutOfStock(
         long pharmacyId, 
         int page,
        int pageSize);

    [Get("/api/ProductControllerForAdmin/GetLowOfStock")]
     Task<List<ProductForPharmacyResponse>> GetLowOfStock(
         long pharmacyId,
        int minimumQuantity, 
         int page,
        int pageSize);

    [Get("/api/ProductControllerForAdmin/GetByPurchasePriceAsync")]
    Task<List<ProductForPharmacyResponse>> GetByPurchasePriceAsync(long pharmacyId,
        decimal price, int page,
        int pageSize);

    [Get("/api/ProductControllerForAdmin/GetByOrderPrice")]
    Task<List<ProductForPharmacyResponse>> GetByOrderPrice(long pharmacyId, decimal price,
        int page,
        int pageSize);

    [Get("/api/ProductControllerForAdmin/GetByCountry")]
    Task<List<ProductForPharmacyResponse>> GetByCountry(long pharmacyId, CountryEnum country,
        int page,
        int pageSize);

}