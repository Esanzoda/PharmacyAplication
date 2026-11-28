using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyService.ApiClient.Clients.IPharmacyApi;
using PharmacyService.Contract.Enums;
using PharmacyService.Contract.Response;


namespace GatewayService.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Position.AdminPharmacy) + nameof(Role.BossPharmacy))]
public class PharmacyController(IPharmacyEndpoint pharmacyEndpoint) : ControllerBase
{
    [HttpPatch]
    public async Task<ActionResult<PharmacyResponse>> UpdateName([FromBody] string newName, long pharmacyId)
    {
        var response = await pharmacyEndpoint.UpdateName(newName, pharmacyId);
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<PharmacyResponse>> UpdateAddress([FromBody] string nawAddress, long pharmacyId)
    {
        var response = await pharmacyEndpoint.UpdateAddress(nawAddress, pharmacyId);
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<PharmacyResponse>> UpdateEmail([FromBody] string newEmail, long pharmacyId)
    {
        var response = await pharmacyEndpoint.UpdateEmail(newEmail, pharmacyId);
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<PharmacyResponse>> UpdatePhoneNumber([FromBody] string nawNumber, long pharmacyId)
    {
        var response = await pharmacyEndpoint.UpdatePhoneNumber(nawNumber, pharmacyId);
        return Ok(response);
    }
}