using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmacyService.Api.Application.CQRS.Deliver.Commands;
using PharmacyService.Api.Application.CQRS.Deliver.Queries;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;

namespace PharmacyService.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DeliverControllerForAdmin(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<DeliverResponse>> Add([FromBody] DeliverRequest request)
    {
        var response = await mediator.Send(new CreateDeliverCommand(request));
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<DeliverResponse>> Update(long id, [FromBody] UpdateDeliverRequest request)
    {
        var response = await mediator.Send(new UpdateDeliverCommand(id, request));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<DeliverResponse>> GetById(long id)
    {
        var response = await mediator.Send(new GetDeliverByIdQuery(id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<DeliverResponse>> GetByEmail(string email)
    {
        var response = await mediator.Send(new GetDeliverByEmailQuery(email));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<DeliverResponse>>> GetAll(
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await mediator.Send(new GetAllDeliverQuery(paginationRequest));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<string>> DeleteById(long id)
    {
        var response = await mediator.Send(new DeleteDeliverCommand(id));
        return Ok(response);
    }
}