using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Pharmacy.DTOs.Request;
using Pharmacy.Domain.Models.Pharmacy.DTOs.Response;
using Refit;

namespace Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

public interface IPharmacyForAdmin
{
    [Post("/api/PharmacyControllerForAdmin/Create")]
    Task<PharmacyResponse> Create(
        PharmacyRequest request);

    [Get("/api/PharmacyControllerForAdmin/GetAll")]
     Task<List<PharmacyResponse>> GetAll(
         [Query]PaginationRequest paginationRequest);

    [Delete("/api/PharmacyControllerForAdmin/Delete")]
    Task<string> Delete(
        long pharmacyId);
}