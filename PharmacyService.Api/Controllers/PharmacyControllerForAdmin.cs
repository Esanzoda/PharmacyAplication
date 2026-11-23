using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmacyService.Api.Application.CQRS.Pharmacy.Commands;
using PharmacyService.Api.Application.CQRS.Pharmacy.Queries;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;

namespace PharmacyService.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PharmacyControllerForAdmin(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<PharmacyResponse>> Create(PharmacyRequest request)
    {
        var response = await mediator.Send(new CreatePharmacyCommand(request));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<PharmacyResponse>>> GetAll(
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await mediator.Send(new GetAllPharmacyQuery(paginationRequest));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<string>> Delete(long pharmacyId)
    {
        var response = await mediator.Send(new DeletePharmacyCommand(pharmacyId));
        return Ok(response);
    }
}