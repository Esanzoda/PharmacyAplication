using PharmacyService.Contract.Request;
using PharmacyService.Contract.Response;
using Refit;

namespace PharmacyService.ApiClient.Clients.IPharmacyApi;

public interface IDeliverForAdmin
{
    [Post("/api/DeliverForAdmin/Add")]
    Task<DeliverResponse> Add(
        [Body] DeliverRequest request);

    [Put("/api/DeliverForAdmin/Update")]
    Task<DeliverResponse> Update(
        long id,
        [Body] UpdateDeliverRequest request);

    [Get("/api/DeliverForAdmin/GetById")]
    Task<DeliverResponse> GetById(
        long id);

    [Get("/api/DeliverForAdmin/GetByEmail")]
    Task<DeliverResponse> GetByEmail(
        string email);

    [Get("/api/DeliverForAdmin/GetAll")]
    Task<List<DeliverResponse>> GetAll(
        [Query] PaginationRequest paginationRequest);

    [Delete("/api/DeliverForAdmin/DeleteById")]
    Task<string> DeleteById(
        long id);
}