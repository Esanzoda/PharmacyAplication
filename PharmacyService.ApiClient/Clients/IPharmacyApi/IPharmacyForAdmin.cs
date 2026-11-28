using PharmacyService.Contract.Request;
using PharmacyService.Contract.Response;
using Refit;

namespace PharmacyService.ApiClient.Clients.IPharmacyApi;

public interface IPharmacyForAdmin
{
    [Post("/api/PharmacyControllerForAdmin/Create")]
    Task<PharmacyResponse> Create(
        PharmacyRequest request);

    [Get("/api/PharmacyControllerForAdmin/GetAll")]
    Task<List<PharmacyResponse>> GetAll(
        [Query] PaginationRequest paginationRequest);

    [Delete("/api/PharmacyControllerForAdmin/Delete")]
    Task<string> Delete(
        long pharmacyId);
}