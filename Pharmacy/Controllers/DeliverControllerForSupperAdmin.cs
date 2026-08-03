using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Deliver.Commands;
using Pharmacy.CQRS.Deliver.Models.DTOs.Request;
using Pharmacy.CQRS.Deliver.Models.DTOs.Response;
using Pharmacy.CQRS.Deliver.Queries;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DeliverControllerForSupperAdmin(IMediator mediator) : ControllerBase
{
    //[Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpPost]
    public async Task<ActionResult<DeliverResponse>> Add([FromBody] DeliverRequest request)
    {
        var response = await mediator.Send(new CreateDeliverCommand(request));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpPut]
    public async Task<ActionResult<DeliverResponse>> Update(long id, [FromBody] UpdateDeliverRequest request)
    {
        var response = await mediator.Send(new UpdateDeliverCommand(id, request));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpGet]
    public async Task<ActionResult<DeliverResponse>> GetById(long id)
    {
        var response = await mediator.Send(new GetDeliverByIdQuery(id));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpGet]
    public async Task<ActionResult<List<DeliverResponse>>> GetByEmail(string email)
    {
        var response = await mediator.Send(new GetDeliverByEmailQuery(email));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpGet]
    public async Task<ActionResult<List<DeliverResponse>>> GetAll(int pageNumber, int pageSize)
    {
        var response = await mediator.Send(new GetAllDeliverQuery(pageNumber, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var response = await mediator.Send(new DeleteDeliverCommand(id));
        return Ok(response);
    }
}