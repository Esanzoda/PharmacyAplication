using Microsoft.AspNetCore.Mvc;
using PharmacyService.ApiClient.Clients.IPharmacyApi;
using PharmacyService.Contract.Enums;
using PharmacyService.Contract.Request;
using PharmacyService.Contract.Response;

namespace GatewayService.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController(
    IAuthEndpoint authEndpoint) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Register([FromBody] CustomerRequest request)
    {
        var response = await authEndpoint.Register(request);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request,
        CancellationToken cancellationToken)
    {
        var response = await authEndpoint.Login(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<LoginResponse>> ReGenerateRefreshToken(string refreshToken,
        CancellationToken cancellationToken)
    {
        var response = await authEndpoint.ReGenerateRefreshToken(refreshToken, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<string>> ForgotPassword(
        string email,
        Role role,
        CancellationToken cancellationToken)
    {
        var response = await authEndpoint.ForgotPassword(email, role, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<string>> ResetPassword(int code,
        string newPassword,
        string email,
        Role role,
        CancellationToken cancellationToken)
    {
        var response = await authEndpoint.ResetPassword(code, newPassword, email, role,
            cancellationToken);

        return Ok(response);
    }
}