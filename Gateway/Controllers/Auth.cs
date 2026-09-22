using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Base.Dto.Response;
using Pharmacy.Domain.Models.Customer.DTOs.Request;
using Pharmacy.Domain.Models.Customer.DTOs.Response;
using Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

namespace Gateway.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class Auth(
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
        var sa = request;
        Console.WriteLine(sa);
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