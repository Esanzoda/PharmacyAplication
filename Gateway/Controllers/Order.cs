using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Domain.Enum;
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
        var customerEmail = User.FindFirstValue(ClaimTypes.Email);
        var response = await orderEndpoint.CancelOrder(customerId, orderId);
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
    public async Task<ActionResult<List<OrderResponseForCustomer>>> GetAllByPagination(int pageNumber, int pageSize)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await orderEndpoint.GetAll(customerId, pageNumber, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponseForCustomer>>> GetByStatusAsync(OrderStatus status, int pageNumber,
        int pageSize)
    {
        var customerId = long.Parse(User.FindFirstValue((ClaimTypes.NameIdentifier))!);
        var response = await orderEndpoint.GetByStatus(customerId, status, pageNumber, pageSize);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<OrderResponseForCustomer>> RemoveItem(long orderId, long productId)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await orderEndpoint.RemoveItem(customerId, orderId, productId);
        return Ok(response);
    }
}