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
    public async Task<ActionResult<LoginResponse>> CustomerLogin(LoginRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new LoginCustomerCommand(request), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<LoginResponse>> EmployeeLogin(LoginRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new LoginEmployeeCommand(request), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<LoginResponse>> DeliverLogin(LoginRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new LoginDeliverCommand(request), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<LoginResponse>> CompanyEmployeeLogin(LoginRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new LoginCompanyEmployeeCommand(request), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<LoginResponse>> ReGenerateRefreshToken(string refreshToken,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new ReGenerateRefreshTokenQuery(refreshToken), cancellationToken);
        return Ok(response);
    }
}