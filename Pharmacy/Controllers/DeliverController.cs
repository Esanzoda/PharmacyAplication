using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Deliver.Commands;
using Pharmacy.CQRS.Deliver.Queries;
using Pharmacy.Domain.Models.Deliver;
using Pharmacy.Domain.Models.Deliver.DTOs.Request;
using Pharmacy.Domain.Models.Deliver.DTOs.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DeliverController(
    IMediator mediator) : ControllerBase
{
    [HttpPut]
    public async Task<ActionResult<DeliverResponse>> Update(long deliverId,[FromBody] UpdateDeliverRequest request)
    {
        var response = await mediator.Send(new UpdateDeliverCommand(deliverId, request));
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<string>> UpdatePassword(long deliverId,string oldPassword, string newPassword)
    {
        var response = await mediator.Send(new UpdateDeliverPasswordCommand(deliverId, oldPassword, newPassword));
        return response;
    }

    [HttpPut]
    public async Task<ActionResult<DeliverResponse>> UpdateOrderStatus(long deliverId,long orderId,
        DeliverUpdateOrderStatus newOrderStatus)
    {
        var response =
            await mediator.Send(new UpdateOrderStatusCommand(deliverId, orderId, newOrderStatus));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<DeliverResponse>>> GetReadyToPickupOrders(int page, int pageSize)
    {
        var response =
            await mediator.Send(new GetOrdersByStatusReadyForPickupQuery(page, pageSize));
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<DeliverResponse>> AcceptOrder(long deliverId,long orderId)
    {
        var response =
            await mediator.Send(new ShippedOrderCommand(orderId, deliverId));
        return Ok(response);
    }
}