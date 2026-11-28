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
[Authorize(Roles = nameof(Role.Employee))]
public class EmployeeController(IEmployeeEndpoint employeeEndpoint) : ControllerBase
{
    [HttpPut]
    public async Task<ActionResult<OrderStatus>> UpdateOrderStatus(long orderId, long pharmacyId,
        OrderStatus newOrderStatus)
    {
        var employeeId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response =
            await employeeEndpoint.UpdateOrderStatus(employeeId, pharmacyId, orderId, newOrderStatus);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<EmployeeResponse>> Update([FromBody] UpdateEmployeeRequest request,
        long pharmacyId)
    {
        var employeeId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await employeeEndpoint.Update(employeeId, request, pharmacyId);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<EmployeeResponse>> GetInformation(long pharmacyId)
    {
        var employeeId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await employeeEndpoint.GetInformation(employeeId, pharmacyId);
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<string>> UpdatePassword(long pharmacyId, string oldPassword,
        string newPassword)
    {
        var employeeId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response =
            await employeeEndpoint.UpdatePassword(employeeId, pharmacyId, oldPassword, newPassword);
        return Ok(response);
    }
}