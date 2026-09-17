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
    [Patch("/api/Customer/UpdateCartItemQuantity")]
    Task UpdateCartItemQuantity(
        long productId, 
        int quantity);


    [Put("/api/Customer/Update")]
    Task<CustomerResponse> Update(
        UpdateCustomerRequest request);


    [Patch("/api/Customer/UpdatePassword")]
    Task<CustomerResponse> UpdatePassword(
        string oldPassword, 
        string newPassword);


    [Patch("/api/Customer/UpdateAddress")]
    Task<string> UpdateAddress(
        string newAddress);



    [Get("/api/Customer/GetCategoryWithProducts")]
    Task<List<ProductForCustomerResponse>> GetCategoryWithProducts(
        int categoryId, int page,
        int pageSize);


    [Post("/api/Customer/AddToCart")]
    Task AddToCart(
        CartItemRequest request);


    [Delete("/api/Customer/RemoveItemFromCart")]
    Task RemoveItemFromCart(
        long productId);


    [Delete("/api/Customer/ClearCart")]
    Task ClearCart();


    [Get("/api/Customer/GetBiCustomerId")]
    Task GetCartByCustomerId();


    [Get("/api/Customer/GetAllPharmacy")]
    Task<PharmacyResponse> GetAllPharmacy(
        int page,
        int pageSize);


    [Get("/api/Customer/GetAllCategory")]
    Task<List<CategoryResponse>> GetAllCategory(
        int page,
        int pageSize);


    [Get("/api/Customer/GetPharmacyProductsByCategory")]
    Task<List<ProductForCustomerResponse>> GetPharmacyProductsByCategory(
        long pharmacyId,
        long categoryId,
        int page,
        int pageSize);

}