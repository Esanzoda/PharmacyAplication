using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Customer.DTOs.Response;
using Refit;

namespace Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

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