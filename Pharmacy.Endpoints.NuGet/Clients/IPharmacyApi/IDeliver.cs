using Pharmacy.Domain.Models.Deliver;
using Pharmacy.Domain.Models.Deliver.DTOs.Request;
using Pharmacy.Domain.Models.Deliver.DTOs.Response;
using Refit;

namespace Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

public interface IDeliver
{
    [Put("api/Deliver/Update")]
    Task<DeliverResponse> Update(
        [Body] UpdateDeliverRequest request);


    [Patch("api/Deliver/UpdatePassword")]
    Task<string> UpdatePassword(
        string oldPassword,
        string newPassword);


    [Put("api/Deliver/UpdateOrderStatus")]
    Task<DeliverResponse> UpdateOrderStatus(
        long orderId,
        DeliverUpdateOrderStatus newOrderStatus);


    [Get("api/Deliver/GetReadyToPickupOrders")]
    Task<List<DeliverResponse>> GetReadyToPickupOrders(
        int page, 
        int pageSize);


    [Patch("api/Deliver/GetOrder")]
    Task<DeliverResponse> GetOrder(
        long orderId);

}