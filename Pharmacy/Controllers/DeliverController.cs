using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Deliver.Commands;
using Pharmacy.CQRS.Deliver.Models.DTOs.Request;
using Pharmacy.CQRS.Deliver.Models.DTOs.Response;
using Pharmacy.CQRS.Deliver.Queries;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Role.Deliver))]
public class DeliverController(
    IMediator mediator) : ControllerBase
{
    [HttpPut]
    public async Task<ActionResult<DeliverResponse>> Update([FromBody] UpdateDeliverRequest request)
    {
        var deliverId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdateDeliverCommand(deliverId, request));
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<string>> UpdatePassword(string oldPassword, string newPassword)
    {
        var deliverId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new UpdateDeliverPasswordCommand(deliverId, oldPassword, newPassword));
        return response;
    }

    [HttpPut]
    public async Task<ActionResult<DeliverResponse>> UpdateOrderStatus(long orderId, OrderStatus newOrderStatus)
    {
        var deliverId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
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
    public async Task<ActionResult<DeliverResponse>> GetOrder(long orderId)
    {
        var deliverId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response =
            await mediator.Send(new ShippedOrderCommand(orderId, deliverId));
        return Ok(response);
    }
}