using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Customer.Models.DTOs.Response;
using Pharmacy.CQRS.Customer.Queries;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Position.AdminPharmacy))]
public class CustomerControllerForAdmin(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetAll(int pageNumber, int pageSize)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new GetAllCustomerByPaginationQuery(pageNumber, pageSize, pharmacyId));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<CustomerResponse>> GetByPhoneAsync(string phone, int page, int pageSize)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new GetCustomerByPhoneNumberQuery(pharmacyId, phone, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetByNameAsync(string name, int page, int pageSize)
    {
        var pharmacyId = long.Parse(User.FindFirstValue("PharmacyId")!);
        var response = await mediator.Send(new GetCustomerByNameQuery(pharmacyId, name, page, pageSize));
        return Ok(response);
    }
}