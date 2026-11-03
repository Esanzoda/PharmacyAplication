using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Deliver;
using Refit;

namespace Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

public interface ISupplier
{
    [Post("/api/Supplier/Add")]
    Task<SupplierResponse> Add(
        SupplierRequest request);

    [Put("/api/Supplier/Update")]
    Task<SupplierResponse> Update(long id,
        UpdateSupplierRequest request);

    [Delete("/api/Supplier/Delete")]
    Task<string> Delete(long id);

    [Get("/api/Supplier/Get")]
    Task<List<SupplierResponse>> Get(
        [Query] PaginationRequest paginationRequest);

    [Get("/api/Supplier/GetById")]
    Task<SupplierRequest> GetById(
        long id);
}