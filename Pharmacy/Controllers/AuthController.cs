using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Auth.Commands;
using Pharmacy.CQRS.Auth.Query;
using Pharmacy.CQRS.Customer.Commands;
using Pharmacy.CQRS.Customer.Models.DTOs.Response;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController(
    IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Register([FromBody] RegisterCommand registerCommandHandler)
    {
        var response = await mediator.Send(registerCommandHandler);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new LoginCommand(request), cancellationToken);
        return Ok(response);
    }


    [HttpPost]
    public async Task<ActionResult<LoginResponse>> ReGenerateRefreshToken(string refreshToken,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new ReGenerateTokenQuery(refreshToken), cancellationToken);
        return Ok(response);
    }
}