using Pharmacy.Domain.Models.Customer.DTOs.Response;
using Refit;

namespace Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

public interface ICustomerForAdmin
{
    [Get("/api/CustomerControllerForAdmin/GetAll")]
    Task<List<CustomerResponse>> GetAll(
        long pharmacyId,
        int pageNumber,
        int pageSize);


    [Get("/api/CustomerControllerForAdmin/GetByPhone")]
    Task<CustomerResponse> GetByPhone(
        long pharmacyId,
        string phone,
        int page,
        int pageSize);


    [Get("/api/CustomerControllerForAdmin/GetByName")]
    Task<List<CustomerResponse>> GetByName(
        long pharmacyId,
        string name,
        int page,
        int pageSize);
}