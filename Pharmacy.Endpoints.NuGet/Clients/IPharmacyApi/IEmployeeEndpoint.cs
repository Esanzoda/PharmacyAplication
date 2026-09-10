using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Employee.DTOs.Request;
using Pharmacy.Domain.Models.Employee.DTOs.Response;
using Refit;

namespace Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

public interface IEmployeeEndpoint
{
    [Put("/api/Employee/UpdateOrderStatus")]
    Task<EmployeeResponse> UpdateOrderStatus(
        long orderId, 
        long pharmacyId,
        OrderStatus newOrderStatus);


    [Put("/api/Employee/Update")]
     Task<EmployeeResponse> Update(
         [Body] UpdateEmployeeRequest request, 
         long pharmacyId);


    [Get("/api/Employee/GetInformation")]
    Task<EmployeeResponse> GetInformation(
        long pharmacyId);


    [Patch("/api/Employee/UpdatePassword")]
   Task<string> UpdatePassword(
       long pharmacyId, 
       string oldPassword, 
       string newPassword);

}