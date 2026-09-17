using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Employee.DTOs.Request;
using Pharmacy.Domain.Models.Employee.DTOs.Response;
using Refit;

namespace Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

public interface IEmployeeForAdmin
{
    [Post("/api/EmployeeControllerForAdmin/Add")]
    Task<EmployeeResponse> Add(
        [Body] EmployeeRequest request, 
        long pharmacyId);


    [Get("/api/EmployeeControllerForAdmin/GetById")]
     Task<EmployeeResponse> GetById(
         long pharmacyId,
         long id);
   

    [Get("/api/EmployeeControllerForAdmin/GetAll")]
   Task<List<EmployeeResponse>> GetAll(
       long pharmacyId, 
       int pageNumber, 
       int pageSize);


    [Delete("/api/EmployeeControllerForAdmin/DeleteById")]
    Task<string> DeleteById(
        long pharmacyId, 
        long id);

    [Get("/api/EmployeeControllerForAdmin/GetByName")]
    Task<List<EmployeeResponse>> GetByName(
        long pharmacyId, 
        string name,
        int page,
        int pageSize);


    [Get("/api/EmployeeControllerForAdmin/GetByAddress")]
    Task<List<EmployeeResponse>> GetByAddress(
        long pharmacyId,
        string address, 
        int page,
        int pageSize);


    [Get("/api/EmployeeControllerForAdmin/GetByNumber")]
    Task<List<EmployeeResponse>> GetByNumber(
        long pharmacyId,
        string number);


    [Get("/api/EmployeeControllerForAdmin/GetByEmail")]
   Task<EmployeeResponse> GetByEmail(
       long pharmacyId, 
       string email);


    [Get("/api/EmployeeControllerForAdmin/GetBySalary")]
     Task<List<EmployeeResponse>> GetBySalary(
         long pharmacyId, 
         decimal salary, 
         int page,
        int pageSize);


    [Get("/api/EmployeeControllerForAdmin/GetByPosition")]
   Task<List<EmployeeResponse>> GetByPosition(
       long pharmacyId,
       Position position,
       int page,
        int pageSize);

}