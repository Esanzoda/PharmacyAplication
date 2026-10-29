using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Customer.DTOs.Response;
using Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

namespace Gateway.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Position.AdminPharmacy))]
public class CustomerForAdmin(ICustomerForAdmin customerForAdmin) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetAll(long pharmacyId,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await customerForAdmin.GetAll(pharmacyId, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<CustomerResponse>> GetByPhone(long pharmacyId, string phone,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await customerForAdmin.GetByPhone(pharmacyId, phone, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetByName(long pharmacyId, string name,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await customerForAdmin.GetByName(pharmacyId, name, paginationRequest);
        return Ok(response);
    }
}