using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Employee.DTOs.Request;
using Pharmacy.Domain.Models.Employee.DTOs.Response;
using Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

namespace Gateway.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Position.AdminPharmacy))]
public class EmployeeForAdmin(IEmployeeForAdmin employeeForAdmin) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<EmployeeResponse>> Add([FromBody] EmployeeRequest request,
        long pharmacyId)
    {
        var response = await employeeForAdmin.Add(request, pharmacyId);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ActionResult<EmployeeResponse>>> GetById(long pharmacyId, long id)
    {
        var response = await employeeForAdmin.GetById(pharmacyId, id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetAll(long pharmacyId,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await employeeForAdmin.GetAll(pharmacyId, paginationRequest);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<string>> DeleteById(long pharmacyId, long id)
    {
        var response = await employeeForAdmin.DeleteById(pharmacyId, id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByName(long pharmacyId, string name,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await employeeForAdmin.GetByName(pharmacyId, name, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByAddress(long pharmacyId,
        string address,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await employeeForAdmin.GetByAddress(pharmacyId,
            address, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByNumber(long pharmacyId, string number)
    {
        var response = await employeeForAdmin.GetByNumber(pharmacyId, number);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<EmployeeResponse>> GetByEmail(long pharmacyId, string email)
    {
        var response = await employeeForAdmin.GetByEmail(pharmacyId, email);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetBySalary(long pharmacyId, decimal salary,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await employeeForAdmin.GetBySalary(pharmacyId,
            salary, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByPosition(long pharmacyId, Position position,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await employeeForAdmin.GetByPosition(pharmacyId,
            position, paginationRequest);
        return Ok(response);
    }
}