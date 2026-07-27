using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Employee.Commands;
using Pharmacy.CQRS.Employee.Queries;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class EmployeeController(IMediator mediator) : ControllerBase
{
    //[Authorize(Roles = nameof(Role.Employee))]
    [HttpPut]
    public async Task<ActionResult<EmployeeResponse>> Update([FromBody] EmployeeRequest request)
    {
        var employeeId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var pharmacyId = 1;
        var response = await mediator.Send(new UpdateEmployeeCommand(pharmacyId, employeeId, request));
        return Ok(response);
    }

    //  [Authorize(Roles = nameof(Role.Employee))]
    [HttpGet]
    public async Task<ActionResult<ActionResult<EmployeeResponse>>> GetInformation()
    {
        var pharmacyId = 1;
        var employeeId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await mediator.Send(new GetEmployeeByIdQuery(pharmacyId, employeeId));
        return Ok(response);
    }
}