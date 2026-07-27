using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Order.Commands;
using Pharmacy.CQRS.Order.Queries;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class OrderController(IMediator mediator) : ControllerBase
{
    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create([FromBody] OrderRequest request)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new CreateOrderCommand(customerId, request));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateFromCart(OrderType orderType, string address)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await mediator.Send(new CreateOrderFromCartCommand(customerId, orderType, address));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpPut]
    public async Task<ActionResult<OrderResponse>> UpdateStatusAsync(long id, [FromBody] UpdateOrderRequest request)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await mediator.Send(new UpdateOrderStatusCommand(customerId, id, request));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpGet("id")]
    public async Task<ActionResult<OrderResponse>> GetByIdAsync(long id)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await mediator.Send(new GetOrderByIdQuery(customerId, id));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetAllByPagination(int pageNumber, int pageSize)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await mediator.Send(new GetAllOrdersQuery(customerId, pageNumber, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetByStatusAsync(OrderStatus status, int pageNumber,
        int pageSize)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await mediator.Send(new GetOrderByOrderStatusQuery(customerId, status, pageNumber, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await mediator.Send(new DeleteOrderCommand(customerId, id));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpDelete]
    public async Task<ActionResult<OrderResponse>> RemoveItem(long orderId, long orderItemId)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new RemoveItemFromOrderCommand(customerId, orderId, orderItemId));
        return Ok(response);
    }
}