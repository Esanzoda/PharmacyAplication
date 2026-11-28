using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyService.ApiClient.Clients.IPharmacyApi;
using PharmacyService.Contract.Enums;
using PharmacyService.Contract.Request;
using PharmacyService.Contract.Response;


namespace GatewayService.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Role.Admin))]
public class PharmacyForAdminController(IPharmacyForAdmin pharmacyForAdmin) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<PharmacyResponse>> Create(PharmacyRequest request)
    {
        var response = await pharmacyForAdmin.Create(request);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<PharmacyResponse>> GetAll(
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await pharmacyForAdmin.GetAll(paginationRequest);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<bool>> Delete(long pharmacyId)
    {
        var response = await pharmacyForAdmin.Delete(pharmacyId);
        return Ok(response);
    }
}