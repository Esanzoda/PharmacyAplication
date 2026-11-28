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
public class DeliverForAdminController(IDeliverForAdmin deliverForAdmin) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<DeliverResponse>> Add([FromBody] DeliverRequest request)
    {
        var response = await deliverForAdmin.Add(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<DeliverResponse>> Update(long id, [FromBody] UpdateDeliverRequest request)
    {
        var response = await deliverForAdmin.Update(id, request);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<DeliverResponse>> GetById(long id)
    {
        var response = await deliverForAdmin.GetById(id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<DeliverResponse>> GetByEmail(string email)
    {
        var response = await deliverForAdmin.GetByEmail(email);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<DeliverResponse>>> GetAll(
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await deliverForAdmin.GetAll(paginationRequest);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<string>> DeleteById(long id)
    {
        var response = await deliverForAdmin.DeleteById(id);
        return Ok(response);
    }
}