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


    [Get("/api/CustomerControllerForAdmin/GetByPhoneAsync")]
    Task<CustomerResponse> GetByPhoneAsync(
        long pharmacyId,
        string phone,
        int page,
        int pageSize);


    [Get("/api/CustomerControllerForAdmin/GetByNameAsync")]
    Task<List<CustomerResponse>> GetByNameAsync(
        long pharmacyId,
        string name,
        int page,
        int pageSize);
}