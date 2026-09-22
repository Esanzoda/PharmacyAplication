using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Deliver;
using Pharmacy.Domain.Models.Deliver.DTOs.Request;
using Pharmacy.Domain.Models.Deliver.DTOs.Response;
using Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;
using Pharmacy.Domain.Models.Order.DTOs.Response;


namespace Gateway.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Role.Deliver))]
public class Deliver(
    IDeliver deliver) : ControllerBase
{
    [HttpPut]
    public async Task<ActionResult<DeliverResponse>> Update([FromBody] UpdateDeliverRequest request)
    {
        var deliverId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await deliver.Update(deliverId, request);
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<string>> UpdatePassword(string oldPassword, string newPassword)
    {
        var deliverId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await deliver.UpdatePassword(deliverId, oldPassword, newPassword);
        return response;
    }

    [HttpPut]
    public async Task<ActionResult<OrderStatus>> UpdateOrderStatus(long orderId,
        DeliverUpdateOrderStatus newOrderStatus)
    {
        var deliverId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response =
            await deliver.UpdateOrderStatus(deliverId, orderId, newOrderStatus);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<DeliverResponse>>> GetReadyToPickupOrders(int page, int pageSize)
    {
        var response =
            await deliver.GetReadyToPickupOrders(page, pageSize);
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<OrderResponseForDeliver>> AcceptOrder(long orderId)
    {
        var deliverId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response =
            await deliver.AcceptOrder(deliverId, orderId);
        return Ok(response);
    }
}