using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Pharmacy.Commands;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Request;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Response;
using Pharmacy.CQRS.Pharmacy.Queries;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Role.Admin))]
public class PharmacyControllerForAdmin(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<PharmacyResponse>> Create(PharmacyRequest request)
    {
        var response = await mediator.Send(new CreatePharmacyCommand(request));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<PharmacyResponse>> GetAll(int page, int pageSize)
    {
        var response = await mediator.Send(new GetAllPharmacyQuery(page, pageSize));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<bool>> Delete(long pharmacyId)
    {
        var response = await mediator.Send(new DeletePharmacyCommand(pharmacyId));
        return Ok(response);
    }
}