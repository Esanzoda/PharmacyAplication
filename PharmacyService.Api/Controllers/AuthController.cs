using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmacyService.Api.Application.CQRS.Auth.Commands;
using PharmacyService.Api.Application.CQRS.Auth.Query;
using PharmacyService.Api.Application.CQRS.Customer.Commands;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;
using Role = PharmacyService.Api.Domain.Enums.Role;


namespace PharmacyService.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController(
    IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Register([FromBody] CustomerRequest request)
    {
        var response = await mediator.Send(new RegisterCommand(request));
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

    [HttpPost]
    public async Task<ActionResult<string>> ForgotPassword(
        string email,
        Role role,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new ForgotCommand(email, role), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<string>> ResetPassword(int code,
        string newPassword,
        string email,
        Role role,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(
            new ResetPasswordCommand(code, newPassword, email, role),
            cancellationToken);

        return Ok(response);
    }
}