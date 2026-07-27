using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Deliver.Commands;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DeliverController(
    IMediator mediator) : ControllerBase
{
    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpPut]
    public async Task<ActionResult<DeliverResponse>> UpdateDeliver(long id, [FromBody] DeliverRequest request)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new UpdateDeliverCommand(pharmacyId, 1, request));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpPut]
    public async Task<ActionResult<DeliverResponse>> UpdateOrderStatus(long orderId, OrderStatus newOrderStatus)
    {
        var deliverId = 1;
        var pharmacyId = 1;
        var response =
            await mediator.Send(new UpdateOrderStatusCommand(pharmacyId, deliverId, orderId, newOrderStatus));
        return Ok(response);
    }
}