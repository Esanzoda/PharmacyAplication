using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Customer.DTOs.Response;
using Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

namespace Gateway.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Position.AdminPharmacy))]
public class CustomerForAdmin(ICustomerForAdmin customerForAdmin) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetAll(long pharmacyId, int pageNumber, int pageSize)
    {
        var response = await customerForAdmin.GetAll(pharmacyId, pageNumber, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<CustomerResponse>> GetByPhone(long pharmacyId, string phone, int page,
        int pageSize)
    {
        var response = await customerForAdmin.GetByPhone(pharmacyId, phone, page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetByName(long pharmacyId, string name, int page,
        int pageSize)
    {
        var response = await customerForAdmin.GetByName(pharmacyId, name, page, pageSize);
        return Ok(response);
    }
}