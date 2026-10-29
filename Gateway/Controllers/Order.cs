using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Order.DTOs.Request;
using Pharmacy.Domain.Models.Order.DTOs.Response;
using Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

namespace Gateway.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Role.Customer))]
public class Order(IOrderEndpoint orderEndpoint) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<List<OrderResponseForCustomer>>> Create([FromBody] CreateOrderRequest request,
        double? newCustomerLat,
        double? newCustomerLong,
        string? newCustomerAddress)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response =
            await orderEndpoint.Create(customerId, request, newCustomerLat, newCustomerLong, newCustomerAddress);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<List<OrderResponseForCustomer>>> CreateFromCart(OrderType orderType,
        double? newCustomerLatitude,
        double? newCustomerLongitude,
        string? newCustomerAddress)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response =
            await orderEndpoint.CreateFromCart(customerId, orderType, newCustomerLatitude, newCustomerLongitude,
                newCustomerAddress);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<OrderResponseForCustomer>> CancelOrder(long orderId)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var customerEmail = User.FindFirstValue(ClaimTypes.Email)!;
        var response = await orderEndpoint.CancelOrder(customerId, customerEmail, orderId);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<OrderResponseForCustomer>> GetInfo(long id)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await orderEndpoint.GetInfo(customerId, id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponseForCustomer>>> GetAllByPagination(
        [FromQuery] PaginationRequest paginationRequest)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await orderEndpoint.GetAll(customerId, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponseForCustomer>>> GetByStatusAsync(OrderStatus status,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await orderEndpoint.GetByStatus(customerId, status, paginationRequest);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<OrderResponseForCustomer>> RemoveItem(long orderId, long orderItemId)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await orderEndpoint.RemoveItem(customerId, orderId, orderItemId);
        return Ok(response);
    }
}