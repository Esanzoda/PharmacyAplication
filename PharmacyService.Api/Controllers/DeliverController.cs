using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmacyService.Api.Application.CQRS.Deliver.Commands;
using PharmacyService.Api.Application.CQRS.Deliver.Queries;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Domain.Enums;

namespace PharmacyService.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DeliverController(
    IMediator mediator) : ControllerBase
{
    [HttpPut]
    public async Task<ActionResult<DeliverResponse>> Update(long deliverId, [FromBody] UpdateDeliverRequest request)
    {
        var response = await mediator.Send(new UpdateDeliverCommand(deliverId, request));
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<string>> UpdatePassword(long deliverId, string oldPassword, string newPassword)
    {
        var response = await mediator.Send(new UpdateDeliverPasswordCommand(deliverId, oldPassword, newPassword));
        return response;
    }

    [HttpPut]
    public async Task<ActionResult<OrderStatus>> UpdateOrderStatus(long deliverId, long orderId,
        DeliverUpdateOrderStatus newOrderStatus)
    {
        var response =
            await mediator.Send(new UpdateOrderStatusCommand(deliverId, orderId, newOrderStatus));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<DeliverResponse>>> GetReadyToPickupOrders(
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await mediator.Send(new GetOrdersByStatusReadyForPickupQuery(paginationRequest));
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<OrderResponseForDeliver>> AcceptOrder(long deliverId, long orderId)
    {
        var response =
            await mediator.Send(new ShippedOrderCommand(orderId, deliverId));
        return Ok(response);
    }
}