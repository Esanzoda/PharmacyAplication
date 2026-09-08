using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Customer.Queries;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Customer.DTOs.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Position.AdminPharmacy))]
public class CustomerControllerForAdmin(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetAll(long pharmacyId, int pageNumber, int pageSize)
    {
        var response = await mediator.Send(new GetAllCustomerByPaginationQuery(pageNumber, pageSize, pharmacyId));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<CustomerResponse>> GetByPhoneAsync(long pharmacyId, string phone, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetCustomerByPhoneNumberQuery(pharmacyId, phone, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetByNameAsync(long pharmacyId, string name, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetCustomerByNameQuery(pharmacyId, name, page, pageSize));
        return Ok(response);
    }
}