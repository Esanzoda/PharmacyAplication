using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Order.Commands;
using Pharmacy.CQRS.Order.Queries;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Order.DTOs.Request;
using Pharmacy.Domain.Models.Order.DTOs.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Role.Customer))]
public class OrderController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<OrderResponseForCustomer>> Create([FromBody] CreateOrderRequest request,
        double? newCustomerLat,
        double? newCustomerLong,
        string? newCustomerAddress)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response =
            await mediator.Send(new CreateOrderCommand(customerId, newCustomerLat, newCustomerLong, newCustomerAddress,
                request));
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponseForCustomer>> CreateFromCart(OrderType orderType,
        double? newCustomerLatitude,
        double? newCustomerLongitude,
        string? newCustomerAddress)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response =
            await mediator.Send(new CreateOrderFromCartCommand(customerId, orderType, newCustomerLatitude,
                newCustomerLongitude,
                newCustomerAddress));
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<OrderResponseForCustomer>> CancelOrder(long orderId)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var customerEmail = User.FindFirstValue(ClaimTypes.Email);
        var response = await mediator.Send(new CancelOrderCommand(customerId, orderId, customerEmail!));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<OrderResponseForCustomer>> GetInfo(long id)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await mediator.Send(new GetOrderByIdQuery(customerId, id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponseForCustomer>>> GetAllByPagination(int pageNumber, int pageSize)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await mediator.Send(new GetAllOrdersQuery(customerId, pageNumber, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponseForCustomer>>> GetByStatusAsync(OrderStatus status, int pageNumber,
        int pageSize)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await mediator.Send(new GetOrderByOrderStatusQuery(customerId, status, pageNumber, pageSize));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<OrderResponseForCustomer>> RemoveItem(long orderId, long productId)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new RemoveItemFromOrderCommand(customerId, orderId, productId));
        return Ok(response);
    }
}