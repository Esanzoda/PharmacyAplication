using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Employee.Commands;
using Pharmacy.CQRS.Employee.Models.DTOs.Request;
using Pharmacy.CQRS.Employee.Models.DTOs.Response;
using Pharmacy.CQRS.Employee.Queries;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Role.Employee))]
public class EmployeeController(IMediator mediator) : ControllerBase
{
    [HttpPut]
    public async Task<ActionResult<EmployeeResponse>> UpdateOrderStatus(long orderId, OrderStatus newOrderStatus)
    {
        var employeeId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response =
            await mediator.Send(new UpdateOrderStatusCommand(employeeId, pharmacyId, orderId, newOrderStatus));
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<EmployeeResponse>> Update([FromBody] UpdateEmployeeRequest request)
    {
        var employeeId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);

        var response = await mediator.Send(new UpdateEmployeeCommand(pharmacyId, employeeId, request));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<EmployeeResponse>> GetInformation()
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var employeeId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new GetEmployeeByIdQuery(pharmacyId, employeeId));
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<string>> UpdatePassword(string oldPassword, string newPassword)
    {
        var employeeId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);

        var response =
            await mediator.Send(new UpdateEmployeePasswordCommand(employeeId, pharmacyId, oldPassword, newPassword));
        return Ok(response);
    }
}