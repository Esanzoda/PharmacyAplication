using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Pharmacy.DTOs.Response;
using Pharmacy.Domain.Models.Product.DTos.Response;
using Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

namespace Gateway.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Position.AdminPharmacy))]
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


    [HttpGet]
    public async Task<ActionResult<List<ExpiredResponse>>> GetExpiryProducts(long pharmacyId, int pageNumber,
        int pageSize)
    {
        var response = await pharmacyEndpoint.GetExpiryProducts(pharmacyId, pageNumber, pageSize);
        return Ok(response);
    }
}