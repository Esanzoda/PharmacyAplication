using Pharmacy.Domain.Models.Cart.DTOs.Request;
using Pharmacy.Domain.Models.Category.DTOs.Response;
using Pharmacy.Domain.Models.Customer.DTOs.Request;
using Pharmacy.Domain.Models.Customer.DTOs.Response;
using Pharmacy.Domain.Models.Pharmacy.DTOs.Response;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using Refit;

namespace Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

public interface ICustomerEndpoint
{
    [Patch("ali/Customer/UpdateCartItemQuantity")]
    Task UpdateCartItemQuantity(
        long productId, 
        int quantity);


    [Put("api/Customer/Update")]
    Task<CustomerResponse> Update(
        UpdateCustomerRequest request);


    [Patch("api/Customer/UpdatePassword")]
    Task<CustomerResponse> UpdatePassword(
        string oldPassword, 
        string newPassword);


    [Patch("api/Customer/UpdateAddress")]
    Task<string> UpdateAddress(
        string newAddress);



    [Get("api/Customer/GetCategoryWithProducts")]
    Task<List<ProductForCustomerResponse>> GetCategoryWithProducts(
        int categoryId, int page,
        int pageSize);


    [Post("api/Customer/AddToCartAsync")]
    Task AddToCartAsync(
        CartItemRequest request);


    [Delete("api/Customer/RemoveItemFromCartAsync")]
    Task RemoveItemFromCartAsync(
        long productId);


    [Delete("api/Customer/ClearCartAsync")]
    Task ClearCartAsync();


    [Get("api/Customer/GetBiCustomerId")]
    Task GetBiCustomerId();


    [Get("api/Customer/GetAllPharmacy")]
    Task<PharmacyResponse> GetAllPharmacy(
        int page,
        int pageSize);


    [Get("api/Customer/GetAllCategory")]
    Task<List<CategoryResponse>> GetAllCategory(
        int page,
        int pageSize);


    [Get("api/Customer/GetPharmacyProductsByCategory")]
    Task<List<ProductForCustomerResponse>> GetPharmacyProductsByCategory(
        long pharmacyId,
        long categoryId,
        int page,
        int pageSize);

}