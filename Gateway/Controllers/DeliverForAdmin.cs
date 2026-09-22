using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Deliver.DTOs.Request;
using Pharmacy.Domain.Models.Deliver.DTOs.Response;
using Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

namespace Gateway.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Role.Admin))]
public class DeliverForAdmin(IDeliverForAdmin deliverForAdmin) : ControllerBase
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
    public async Task<ActionResult<List<DeliverResponse>>> GetAll(int pageNumber, int pageSize)
    {
        var response = await deliverForAdmin.GetAll(pageNumber, pageSize);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<string>> DeleteById(long id)
    {
        var response = await deliverForAdmin.DeleteById(id);
        return Ok(response);
    }
}