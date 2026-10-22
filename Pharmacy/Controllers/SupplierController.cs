using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Supplier.Commands;
using Pharmacy.CQRS.Supplier.Queries;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Deliver;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class SupplierController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<SupplierResponse>> Add(SupplierRequest request)
    {
        var response = await mediator.Send(new CreateSupplierCommand(request));
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<SupplierResponse>> Update(long id,
        UpdateSupplierRequest request)
    {
        var response = await mediator.Send(new UpdateSupplierCommand(id, request));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<string>> Delete(long id)
    {
        var response = await mediator.Send(new DeleteSupplierCommand(id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<SupplierResponse>>> Get([FromQuery] PaginationRequest paginationRequest)
    {
        var response = await mediator.Send(new GetSuppliersQuery(paginationRequest));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<SupplierResponse>> GetById(long id)
    {
        var response = await mediator.Send(new GetSupplierByIdQuery(id));
        return Ok(response);
    }
}