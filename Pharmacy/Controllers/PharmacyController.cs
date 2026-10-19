using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Pharmacy.Commands;
using Pharmacy.Domain.Models.Pharmacy.DTOs.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PharmacyController(IMediator mediator) : ControllerBase
{
    [HttpPatch]
    public async Task<ActionResult<PharmacyResponse>> UpdateName([FromBody] string newName, long pharmacyId)
    {
        var response = await mediator.Send(new UpdatePharmacyNameCommand(pharmacyId, newName));
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<PharmacyResponse>> UpdateAddress([FromBody] string nawAddress, long pharmacyId)
    {
        var response = await mediator.Send(new UpdatePharmacyAddressCommand(pharmacyId, nawAddress));
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<PharmacyResponse>> UpdateEmail([FromBody] string newEmail, long pharmacyId)
    {
        var response = await mediator.Send(new UpdatePharmacyEmailCommand(pharmacyId, newEmail));
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<PharmacyResponse>> UpdatePhoneNumber([FromBody] string nawNumber, long pharmacyId)
    {
        var response = await mediator.Send(new UpdatePharmacyPhoneNumberCommand(pharmacyId, nawNumber));
        return Ok(response);
    }
}