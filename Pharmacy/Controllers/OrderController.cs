using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Order.Commands;
using Pharmacy.CQRS.Order.Queries;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Order.DTOs.Request;
using Pharmacy.Domain.Models.Order.DTOs.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class OrderController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<OrderResponseForCustomer>> Create(long customerId,[FromBody] CreateOrderRequest request,
        double? newCustomerLat,
        double? newCustomerLong,
        string? newCustomerAddress)
    {
        var response =
            await mediator.Send(new CreateOrderCommand(customerId, newCustomerLat, newCustomerLong, newCustomerAddress,
                request));
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponseForCustomer>> CreateFromCart(long customerId,OrderType orderType,
        double? newCustomerLatitude,
        double? newCustomerLongitude,
        string? newCustomerAddress)
    {
        var response =
            await mediator.Send(new CreateOrderFromCartCommand(customerId, orderType, newCustomerLatitude,
                newCustomerLongitude,
                newCustomerAddress));
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<OrderResponseForCustomer>> CancelOrder(long customerId,long orderId)
    {
        var customerEmail = User.FindFirstValue(ClaimTypes.Email);
        var response = await mediator.Send(new CancelOrderCommand(customerId, orderId, customerEmail!));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<OrderResponseForCustomer>> GetInfo(long customerId,long id)
    {
        var response = await mediator.Send(new GetOrderByIdQuery(customerId, id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponseForCustomer>>> GetAll(long customerId,int pageNumber, int pageSize)
    {
        var response = await mediator.Send(new GetAllOrdersQuery(customerId, pageNumber, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponseForCustomer>>> GetByStatus(long customerId,OrderStatus status, int pageNumber,
        int pageSize)
    {
        var response = await mediator.Send(new GetOrderByOrderStatusQuery(customerId, status, pageNumber, pageSize));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<OrderResponseForCustomer>> RemoveItem(long customerId,long orderId, long productId)
    {
        var response = await mediator.Send(new RemoveItemFromOrderCommand(customerId, orderId, productId));
        return Ok(response);
    }
}