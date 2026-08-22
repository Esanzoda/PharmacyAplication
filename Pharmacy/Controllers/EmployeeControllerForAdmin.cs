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
[Authorize(Roles = nameof(Position.AdminPharmacy))]
public class EmployeeControllerForAdmin(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<EmployeeResponse>> Add([FromBody] EmployeeRequest request)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new CreateEmployeeCommand(pharmacyId, request));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ActionResult<EmployeeResponse>>> GetById(long id)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new GetEmployeeByIdQuery(pharmacyId, id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetAll(int pageNumber, int pageSize)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new GetAllEmployeeByPaginationQuery(pharmacyId, pageNumber, pageSize));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new DeleteEmployeeCommand(pharmacyId, id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByName(string name, int page, int pageSize)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new GetEmployeesByNameQuery(pharmacyId, name, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByAddress(string address, int page,
        int pageSize)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new GetEmployeesByAddressQuery(pharmacyId, address, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByNumber(string number)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new GetEmployeeByNumberQuery(pharmacyId, number));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<EmployeeResponse>> GetByEmail(string email)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new GetEmployeeByEmailQuery(pharmacyId, email));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetBySalary(decimal salary, int page,
        int pageSize)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new GetEmployeesBySalaryQuery(pharmacyId, salary, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetByPosition(Position position, int page, int pageSize)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new GetEmployeesByPositionQuery(pharmacyId, position, page, pageSize));
        return Ok(response);
    }
}