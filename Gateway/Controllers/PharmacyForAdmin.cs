using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Pharmacy.DTOs.Request;
using Pharmacy.Domain.Models.Pharmacy.DTOs.Response;
using Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

namespace Gateway.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Role.Admin))]
public class PharmacyForAdmin(IPharmacyForAdmin pharmacyForAdmin) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<PharmacyResponse>> Create(PharmacyRequest request)
    {
        var response = await pharmacyForAdmin.Create(request);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<PharmacyResponse>> GetAll(int page, int pageSize)
    {
        var response = await pharmacyForAdmin.GetAll(page, pageSize);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<bool>> Delete(long pharmacyId)
    {
        var response = await pharmacyForAdmin.Delete(pharmacyId);
        return Ok(response);
    }
}