using Pharmacy.Domain.Models.Product.DTos.Request;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using Refit;

namespace Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

public interface IProductForAdminEndpoint
{
    [Post("/api/ProductControllerForAdmin/Add")]
    Task<ProductForCustomerResponse> Add(
        [Body] ProductRequest request,
        long pharmacyId);

    [Put("/api/ProductControllerForAdmin/Update")]
    Task<ProductForCustomerResponse> Update(
        long id, 
        [Body] UpdateProductRequest request,
        long pharmacyId);

    [Delete("/api/ProductControllerForAdmin/DeleteById")]
    Task<string> DeleteById(
        long id, 
        long pharmacyId);

    [Get("/api/ProductControllerForAdmin/GetById")]
   Task<ProductForCustomerResponse> GetById(
       long id, 
       long pharmacyId);

    [Get("/api/ProductControllerForAdmin/GetAll")]
   Task<List<ProductForCustomerResponse>> GetAll(
       long pharmacyId,
       int pageNumber,
        int pageSize);

    [Get("/api/ProductControllerForAdmin/GetByBarcode")]
     Task<ProductForCustomerResponse> GetByBarcode(
         long pharmacyId, 
         string barcode);

    [Get("/api/ProductControllerForAdmin/GetByName")]
    Task<List<ProductForCustomerResponse>> GetByName(
        long pharmacyId, 
        string name,
        int page,
        int pageSize);

    [Get("/api/ProductControllerForAdmin/GetByCategory")]
     Task<List<ProductForCustomerResponse>> GetByCategory(
         long pharmacyId, 
         long categoryId,
        int pageNumber, 
         int pageSize);

    [Get("/api/ProductControllerForAdmin/GetOutOfStock")]
     Task<List<ProductForCustomerResponse>> GetOutOfStock(
         long pharmacyId, 
         int page,
        int pageSize);

    [Get("/api/ProductControllerForAdmin/GetLowOfStock")]
     Task<List<ProductForCustomerResponse>> GetLowOfStock(
         long pharmacyId,
        int minimumQuantity, 
         int page,
        int pageSize);
}