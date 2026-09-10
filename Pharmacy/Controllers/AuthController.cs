using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Auth.Commands;
using Pharmacy.CQRS.Auth.Query;
using Pharmacy.CQRS.Customer.Commands;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Base.Dto.Response;
using Pharmacy.Domain.Models.Customer.DTOs.Request;
using Pharmacy.Domain.Models.Customer.DTOs.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController(
    IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Register([FromBody] CustomerRequest request)
    {
        var response = await mediator.Send(new RegisterCommand( request));
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
    public async Task<ActionResult<LoginResponse>> ForgotPassword(
        string email,
        Role role,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new ForgotCommand(email, role), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<LoginResponse>> ResetPassword(int code,
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