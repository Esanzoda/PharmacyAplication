using System.Globalization;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Order.Commands;
using Pharmacy.CQRS.Order.Models.DTOs.Request;
using Pharmacy.CQRS.Order.Models.DTOs.Response;
using Pharmacy.CQRS.Order.Queries;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class OrderController(IMediator mediator) : ControllerBase
{
    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create([FromBody] OrderRequest request)
    {
        var customerId = 4; //long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var customerLat = 40.3058741; //double.Parse(User.FindFirstValue("Latitude")!, CultureInfo.InvariantCulture);
        var customerLong = 69.6295725; //double.Parse(User.FindFirstValue("Longitude")!, CultureInfo.InvariantCulture);;
        var response = await mediator.Send(new CreateOrderCommand(customerId, customerLat, customerLong, request));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateFromCart(OrderType orderType)
    {
        var customerId = 4; //long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var customerLat = 40.3058741; //double.Parse(User.FindFirstValue("Latitude")!, CultureInfo.InvariantCulture);
        var customerLong = 69.6295725; //double.Parse(User.FindFirstValue("Longitude")!, CultureInfo.InvariantCulture);
        var response =
            await mediator.Send(new CreateOrderFromCartCommand(customerId, orderType, customerLat, customerLong));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpPut]
    public async Task<ActionResult<OrderResponse>> UpdateStatusAsync(long orderId, [FromQuery] OrderStatus orderStatus)
    {
        var customerId = 4; //long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await mediator.Send(new CancelOrderCommand(customerId, orderId, orderStatus));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpGet]
    public async Task<ActionResult<OrderResponse>> GetByIdAsync(long id)
    {
        var customerId = 4; //long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await mediator.Send(new GetOrderByIdQuery(customerId, id));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetAllByPagination(int pageNumber, int pageSize)
    {
        var customerId = 4; //long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await mediator.Send(new GetAllOrdersQuery(customerId, pageNumber, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetByStatusAsync(OrderStatus status, int pageNumber,
        int pageSize)
    {
        var customerId = 1; //long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await mediator.Send(new GetOrderByOrderStatusQuery(customerId, status, pageNumber, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpDelete]
    public async Task<ActionResult<OrderResponse>> RemoveItem(long orderId, long productId)
    {
        var customerId = 4; //long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new RemoveItemFromOrderCommand(customerId, orderId, productId));
        return Ok(response);
    }
}