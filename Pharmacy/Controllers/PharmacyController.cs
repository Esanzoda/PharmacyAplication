using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Pharmacy.Commands;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Request;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PharmacyController(IMediator mediator) : ControllerBase
{
    // [Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpPost]
    public async Task<ActionResult<PharmacyResponse>> Create(PharmacyRequest request)
    {
        var response = await mediator.Send(new CreatePharmacyCommand(request));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpPatch]
    public async Task<ActionResult<PharmacyResponse>> UpdateName([FromBody] string newName)
    {
        var pharmacyId = 1; //long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdatePharmacyNameCommand(pharmacyId, newName));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpPatch]
    public async Task<ActionResult<PharmacyResponse>> UpdateAddress([FromBody] string nawAddress)
    {
        var pharmacyId = 1; //long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdatePharmacyAddressCommand(pharmacyId, nawAddress));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpPatch]
    public async Task<ActionResult<PharmacyResponse>> UpdateEmail([FromBody] string newEmail)
    {
        var pharmacyId = 1; //long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdatePharmacyEmailCommand(pharmacyId, newEmail));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpPatch]
    public async Task<ActionResult<PharmacyResponse>> UpdatePhoneNumber([FromBody] string nawNumber)
    {
        var pharmacyId = 1; //long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdatePharmacyPhoneNumberCommand(pharmacyId, nawNumber));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpPut]
    public async Task<ActionResult<PharmacyResponse>> Update([FromBody] PharmacyRequest request)
    {
        var pharmacyId = 1; //long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdatePharmacyCommand(pharmacyId, request));
        return Ok(response);
    }
}