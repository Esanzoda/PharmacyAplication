using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Employee.Commands;
using Pharmacy.CQRS.Employee.Queries;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Employee.DTOs.Request;
using Pharmacy.Domain.Models.Employee.DTOs.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Position.AdminPharmacy))]
public class EmployeeControllerForAdmin(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<EmployeeResponse>> Add([FromBody] EmployeeRequest request, long pharmacyId)
    {
        var response = await mediator.Send(new CreateEmployeeCommand(pharmacyId, request));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ActionResult<EmployeeResponse>>> GetById(long pharmacyId, long id)
    {
        var response = await mediator.Send(new GetEmployeeByIdQuery(pharmacyId, id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetAll(long pharmacyId, int pageNumber, int pageSize)
    {
        var response = await mediator.Send(new GetAllEmployeeByPaginationQuery(pharmacyId, pageNumber, pageSize));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteById(long pharmacyId, long id)
    {
        var response = await mediator.Send(new DeleteEmployeeCommand(pharmacyId, id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByName(long pharmacyId, string name, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetEmployeesByNameQuery(pharmacyId, name, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByAddress(long pharmacyId, string address, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetEmployeesByAddressQuery(pharmacyId, address, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByNumber(long pharmacyId, string number)
    {
        var response = await mediator.Send(new GetEmployeeByNumberQuery(pharmacyId, number));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<EmployeeResponse>> GetByEmail(long pharmacyId, string email)
    {
        var response = await mediator.Send(new GetEmployeeByEmailQuery(pharmacyId, email));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetBySalary(long pharmacyId, decimal salary, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetEmployeesBySalaryQuery(pharmacyId, salary, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByPosition(long pharmacyId, Position position, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetEmployeesByPositionQuery(pharmacyId, position, page, pageSize));
        return Ok(response);
    }
}