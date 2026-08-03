using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Deliver.Commands;
using Pharmacy.CQRS.Deliver.Models.DTOs.Request;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DeliverController(
    IMediator mediator) : ControllerBase
{
    // [Authorize(Roles = nameof(Role.Deliver))]
    [HttpPut]
    public async Task<ActionResult<DeliverResponse>> Update([FromBody] UpdateDeliverRequest request)
    {
        var deliverId = 2;
        var response = await mediator.Send(new UpdateDeliverCommand(deliverId, request));
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<string>> UpdatePassword(string oldPassword, string newPassword)
    {
        var deliverId = 1;
        var response = await mediator.Send(new UpdateDeliverPasswordCommand(deliverId, oldPassword, newPassword));
        return response;
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpPut]
    public async Task<ActionResult<DeliverResponse>> UpdateOrderStatus(long orderId, OrderStatus newOrderStatus)
    {
        var deliverId = 1;
        var response =
            await mediator.Send(new UpdateOrderStatusCommand(deliverId, orderId, newOrderStatus));
        return Ok(response);
    }
}

public class DeliverResponse
{
}