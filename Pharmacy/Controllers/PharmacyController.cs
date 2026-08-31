using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.ExpiredProducts.Query;
using Pharmacy.CQRS.Pharmacy.Commands;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Response;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Position.AdminPharmacy))]
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


    [HttpGet]
    public async Task<ActionResult<List<ExpiredResponse>>> GetExpiryProducts(long pharmacyId, int pageNumber,
        int pageSize)
    {
        var response = await mediator.Send(new GetExpiryProductsQuery(pharmacyId, pageNumber, pageSize));
        return Ok(response);
    }
}