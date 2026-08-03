using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Customer.Models.DTOs.Response;
using Pharmacy.CQRS.Customer.Queries;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CustomerControllerForAdmin(IMediator mediator) : ControllerBase
{
    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetAll(int pageNumber, int pageSize)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetAllCustomerByPaginationQuery(pageNumber, pageSize, pharmacyId),
            HttpContext.RequestAborted);
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<CustomerResponse>> GetByPhoneAsync(string phone)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetCustomerByPhoneNumberQuery(pharmacyId, phone));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetByNameAsync(string name)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetCustomerByNameQuery(pharmacyId, name));
        return Ok(response);
    }
}