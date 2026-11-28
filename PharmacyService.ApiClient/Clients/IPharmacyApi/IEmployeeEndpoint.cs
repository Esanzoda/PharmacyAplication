using PharmacyService.Contract.Enums;
using PharmacyService.Contract.Request;
using PharmacyService.Contract.Response;
using Refit;

namespace PharmacyService.ApiClient.Clients.IPharmacyApi;

public interface IEmployeeEndpoint
{
    [Put("/api/Employee/UpdateOrderStatus")]
    Task<OrderStatus> UpdateOrderStatus(
        long employeeId,
        long orderId,
        long pharmacyId,
        OrderStatus newOrderStatus);

    [Put("/api/Employee/Update")]
    Task<EmployeeResponse> Update(
        long employeeId,
        [Body] UpdateEmployeeRequest request,
        long pharmacyId);

    [Get("/api/Employee/GetInformation")]
    Task<EmployeeResponse> GetInformation(
        long employeeId,
        long pharmacyId);

    [Patch("/api/Employee/UpdatePassword")]
    Task<string> UpdatePassword(
        long employeeId,
        long pharmacyId,
        string oldPassword,
        string newPassword);
}