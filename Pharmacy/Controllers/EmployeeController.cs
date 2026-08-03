using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Employee.Commands;
using Pharmacy.CQRS.Employee.Models.DTOs.Request;
using Pharmacy.CQRS.Employee.Models.DTOs.Response;
using Pharmacy.CQRS.Employee.Queries;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class EmployeeController(IMediator mediator) : ControllerBase
{
    //[Authorize(Roles = nameof(Role.Employee))]
    [HttpPut]
    public async Task<ActionResult<EmployeeResponse>> Update([FromBody] UpdateEmployeeRequest request)
    {
        var employeeId = 1; //long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var pharmacyId = 1;
        var response = await mediator.Send(new UpdateEmployeeCommand(pharmacyId, employeeId, request));
        return Ok(response);
    }

    //  [Authorize(Roles = nameof(Role.Employee))]
    [HttpGet]
    public async Task<ActionResult<EmployeeResponse>> GetInformation()
    {
        var pharmacyId = 1;
        var employeeId = 1; //long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new GetEmployeeByIdQuery(pharmacyId, employeeId));
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<string>> UpdatePassword(string oldPassword, string newPassword)
    {
        var employeeId = 1;
        var pharmacyId = 1;
        var response =
            await mediator.Send(new UpdateEmployeePasswordCommand(employeeId, pharmacyId, oldPassword, newPassword));
        return Ok(response);
    }
}