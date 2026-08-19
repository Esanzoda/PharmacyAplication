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
//[Authorize(Roles = nameof(Role.Customer))]
public class OrderController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create([FromBody] OrderRequest request)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var customerLat = double.Parse(User.FindFirstValue("Latitude")!, CultureInfo.InvariantCulture);
        var customerLong = double.Parse(User.FindFirstValue("Longitude")!, CultureInfo.InvariantCulture);
        var customerEmail = User.FindFirstValue(ClaimTypes.Email);
        var customerAddress = User.FindFirstValue(ClaimTypes.StreetAddress);
        var response = await mediator.Send(new CreateOrderCommand(customerId, customerLat, customerLong, request,
            customerEmail!, customerAddress!));
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateFromCart(OrderType orderType)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var customerLat = double.Parse(User.FindFirstValue("Latitude")!);
        var customerLong = double.Parse(User.FindFirstValue("Longitude")!);
        var response =
            await mediator.Send(new CreateOrderFromCartCommand(customerId, orderType, customerLat, customerLong));
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<OrderResponse>> CancelOrder(long orderId)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var customerEmail = User.FindFirstValue(ClaimTypes.Email);
        var response = await mediator.Send(new CancelOrderCommand(customerId, orderId, customerEmail!));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<OrderResponse>> GetInfo(long id)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await mediator.Send(new GetOrderByIdQuery(customerId, id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetAllByPagination(int pageNumber, int pageSize)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await mediator.Send(new GetAllOrdersQuery(customerId, pageNumber, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetByStatusAsync(OrderStatus status, int pageNumber,
        int pageSize)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await mediator.Send(new GetOrderByOrderStatusQuery(customerId, status, pageNumber, pageSize));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<OrderResponse>> RemoveItem(long orderId, long productId)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new RemoveItemFromOrderCommand(customerId, orderId, productId));
        return Ok(response);
    }
}