using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Employee.Commands;
using Pharmacy.CQRS.Employee.Queries;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Employee.DTOs.Request;
using Pharmacy.Domain.Models.Employee.DTOs.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class EmployeeController(IMediator mediator) : ControllerBase
{
    [HttpPut]
    public async Task<ActionResult<OrderStatus>> UpdateOrderStatus(long employeeId,long orderId, long pharmacyId,
        OrderStatus newOrderStatus)
    {
        var response =
            await mediator.Send(new UpdateOrderStatusCommand(employeeId, pharmacyId, orderId, newOrderStatus));
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<EmployeeResponse>> Update(long employeeId,[FromBody] UpdateEmployeeRequest request, long pharmacyId)
    {
        var response = await mediator.Send(new UpdateEmployeeCommand(pharmacyId, employeeId, request));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<EmployeeResponse>> GetInformation(long employeeId,long pharmacyId)
    {
        var response = await mediator.Send(new GetEmployeeByIdQuery(pharmacyId, employeeId));
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<string>> UpdatePassword(long employeeId,long pharmacyId, string oldPassword, string newPassword)
    {
        var response =
            await mediator.Send(new UpdateEmployeePasswordCommand(employeeId, pharmacyId, oldPassword, newPassword));
        return Ok(response);
    }
}