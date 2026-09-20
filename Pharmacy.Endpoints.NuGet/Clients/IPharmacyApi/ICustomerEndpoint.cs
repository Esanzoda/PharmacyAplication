using Pharmacy.Domain.Models.Cart.DTOs.Request;
using Pharmacy.Domain.Models.Cart.DTOs.Response;
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
    Task<CartItemResponse> UpdateCartItemQuantity(
        long customerId,
        long productId, 
        int quantity);


    [Put("/api/Customer/Update")]
    Task<CustomerResponse> Update(
        long customerId,
        UpdateCustomerRequest request);


    [Patch("/api/Customer/UpdatePassword")]
    Task<CustomerResponse> UpdatePassword(
        long customerId,
        string oldPassword, 
        string newPassword);


    [Patch("/api/Customer/UpdateAddress")]
    Task<string> UpdateAddress(
        long customerId,
        string newAddress);



    [Get("/api/Customer/GetCategoryWithProducts")]
    Task<List<ProductForCustomerResponse>> GetCategoryWithProducts(
        int categoryId, int page,
        int pageSize);


    [Post("/api/Customer/AddToCart")]
    Task<CartResponse> AddToCart(
        long customerId,
        CartItemRequest request);


    [Delete("/api/Customer/RemoveItemFromCart")]
    Task<CartResponse> RemoveItemFromCart(
        long customerId,
        long productId);


    [Delete("/api/Customer/ClearCart")]
    Task<CartResponse> ClearCart(
        long customerId);


    [Get("/api/Customer/GetCartByCustomerId")]
    Task<CartResponse> GetCartByCustomerId(
        long customerId);


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