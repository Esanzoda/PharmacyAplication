using Pharmacy.Domain.Models.Product.DTos.Request;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using Refit;

namespace Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

public interface IProductForAdminEndpoint
{
    [Post("api/ProductControllerForAdmin/")]
    Task<ProductForCustomerResponse> Add(
        [Body] ProductRequest request,
        long pharmacyId);

    [Put("api/ProductControllerForAdmin/")]
    Task<ProductForCustomerResponse> Update(
        long id, 
        [Body] UpdateProductRequest request,
        long pharmacyId);

    [Delete("api/ProductControllerForAdmin/")]
    Task DeleteById(
        long id, 
        long pharmacyId);

    [Get("api/ProductControllerForAdmin/")]
   Task<ProductForCustomerResponse> GetById(
       long id, 
       long pharmacyId);

    [Get("api/ProductControllerForAdmin/")]
   Task<List<ProductForCustomerResponse>> GetAll(
       long pharmacyId,
       int pageNumber,
        int pageSize);

    [Get("api/ProductControllerForAdmin/")]
     Task<ProductForCustomerResponse> GetByBarcodeAsync(
         long pharmacyId, 
         string barcode);

    [Get("api/ProductControllerForAdmin/")]
    Task<List<ProductForCustomerResponse>> GetByName(
        long pharmacyId, 
        string name,
        int page,
        int pageSize);

    [Get("api/ProductControllerForAdmin/")]
     Task<List<ProductForCustomerResponse>> GetByCategory(
         long pharmacyId, 
         long categoryId,
        int pageNumber, 
         int pageSize);

    [Get("api/ProductControllerForAdmin/")]
     Task<List<ProductForCustomerResponse>> GetOutOfStockAsync(
         long pharmacyId, 
         int page,
        int pageSize);

    [Get("api/ProductControllerForAdmin/")]
     Task<List<ProductForCustomerResponse>> GetLowOfStockAsync(
         long pharmacyId,
        int minimumQuantity, 
         int page,
        int pageSize);
}