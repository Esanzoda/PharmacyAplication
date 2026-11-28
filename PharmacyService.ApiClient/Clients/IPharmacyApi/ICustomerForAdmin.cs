using PharmacyService.Contract.Request;
using PharmacyService.Contract.Response;
using Refit;

namespace PharmacyService.ApiClient.Clients.IPharmacyApi;

public interface ICustomerForAdmin
{
    [Get("/api/CustomerControllerForAdmin/GetAll")]
    Task<List<CustomerResponse>> GetAll(
        long pharmacyId,
        [Query] PaginationRequest paginationRequest);

    [Get("/api/CustomerControllerForAdmin/GetByPhone")]
    Task<CustomerResponse> GetByPhone(
        long pharmacyId,
        string phone,
        [Query] PaginationRequest paginationRequest);


    [Get("/api/CustomerControllerForAdmin/GetByName")]
    Task<List<CustomerResponse>> GetByName(
        long pharmacyId,
        string name,
        [Query] PaginationRequest paginationRequest);
}