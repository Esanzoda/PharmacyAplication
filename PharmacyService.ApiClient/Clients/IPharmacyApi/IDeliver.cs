using PharmacyService.Contract.Enums;
using PharmacyService.Contract.Request;
using PharmacyService.Contract.Response;
using Refit;

namespace PharmacyService.ApiClient.Clients.IPharmacyApi;

public interface IDeliver
{
    [Put("/api/Deliver/Update")]
    Task<DeliverResponse> Update(
        long deliverId,
        [Body] UpdateDeliverRequest request);

    [Patch("/api/Deliver/UpdatePassword")]
    Task<string> UpdatePassword(
        long deliverId,
        string oldPassword,
        string newPassword);

    [Put("/api/Deliver/UpdateOrderStatus")]
    Task<OrderStatus> UpdateOrderStatus(
        long deliverId,
        long orderId,
        DeliverUpdateOrderStatus newOrderStatus);

    [Get("/api/Deliver/GetReadyToPickupOrders")]
    Task<List<DeliverResponse>> GetReadyToPickupOrders(
        [Query] PaginationRequest paginationRequest);

    [Patch("/api/Deliver/AcceptOrder")]
    Task<OrderResponseForDeliver> AcceptOrder(
        long deliverId,
        long orderId);
}