using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Auth.Commands;
using Pharmacy.CQRS.Customer.Commands;
using Pharmacy.CQRS.Customer.Models.DTOs.Response;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Register([FromBody] RegisterCommand registerCommandHandler)
    {
        var response = await mediator.Send(registerCommandHandler);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<LoginResponse>> Login(LoginCommand request)
    {
        var response = await mediator.Send(request);
        return Ok(response);
    }

    // [Authorize]
    [HttpPost]
    public async Task<ActionResult<LoginResponse>> ReCreateToken(ReGenerateRefreshTokenCommand request)
    {
        var response = await mediator.Send(request);
        return Ok(response);
    }
}