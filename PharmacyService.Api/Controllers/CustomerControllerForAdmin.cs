using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmacyService.Api.Application.CQRS.Customer.Queries;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;

namespace PharmacyService.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CustomerControllerForAdmin(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetAll(long pharmacyId,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await mediator.Send(
            new GetAllCustomerByPaginationQuery(pharmacyId, paginationRequest));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<CustomerResponse>> GetByPhone(long pharmacyId, string phone,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await mediator.Send(
            new GetCustomerByPhoneNumberQuery(pharmacyId, phone, paginationRequest));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetByName(long pharmacyId, string name,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await mediator.Send(
            new GetCustomerByNameQuery(pharmacyId, name, paginationRequest));
        return Ok(response);
    }
}