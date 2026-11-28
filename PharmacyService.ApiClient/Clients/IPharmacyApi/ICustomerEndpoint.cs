using PharmacyService.Contract.Request;
using PharmacyService.Contract.Response;
using Refit;

namespace PharmacyService.ApiClient.Clients.IPharmacyApi;

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
    Task<List<PharmacyResponse>> GetAllPharmacy(
        [Query] PaginationRequest paginationRequest);
}