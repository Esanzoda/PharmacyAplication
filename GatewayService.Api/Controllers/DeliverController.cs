using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyService.ApiClient.Clients.IPharmacyApi;
using PharmacyService.Contract.Enums;
using PharmacyService.Contract.Request;
using PharmacyService.Contract.Response;


namespace GatewayService.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Role.Deliver))]
public class DeliverController(
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
    public async Task<ActionResult<List<DeliverResponse>>> GetReadyToPickupOrders(
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await deliver.GetReadyToPickupOrders(paginationRequest);
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