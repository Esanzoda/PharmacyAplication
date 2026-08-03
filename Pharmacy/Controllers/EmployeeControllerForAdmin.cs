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
public class EmployeeControllerForAdmin(IMediator mediator) : ControllerBase
{
    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpPost]
    public async Task<ActionResult<EmployeeResponse>> Add([FromBody] EmployeeRequest request)
    {
        var pharmacyId = 2;
        var response = await mediator.Send(new CreateEmployeeCommand(pharmacyId, request));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<ActionResult<EmployeeResponse>>> GetById(long id)
    {
        var pharmacyId = 2;
        var response = await mediator.Send(new GetEmployeeByIdQuery(pharmacyId, id));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetAll(int pageNumber, int pageSize)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetAllEmployeeByPaginationQuery(pharmacyId, pageNumber, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new DeleteEmployeeCommand(pharmacyId, id));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByNameAsync(string name, int page, int pageSize)
    {
        var pharmacyId = 2;
        var response = await mediator.Send(new GetEmployeesByNameQuery(pharmacyId, name, page, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByAddressAsync(string address, int page,
        int pageSize)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetEmployeesByAddressQuery(pharmacyId, address, page, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByNumberAsync(string number)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetEmployeeByNumberQuery(pharmacyId, number));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<EmployeeResponse>> GetByEmailAsync(string email)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetEmployeeByEmailQuery(pharmacyId, email));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetBySalaryAsync(decimal salary, int page,
        int pageSize)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetEmployeesBySalaryQuery(pharmacyId, salary, page, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByPosition(Role role, int page, int pageSize)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetEmployeesByRoleQuery(pharmacyId, role, page, pageSize));
        return Ok(response);
    }
}