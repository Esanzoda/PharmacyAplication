using PharmacyService.Contract.Enums;
using PharmacyService.Contract.Request;
using PharmacyService.Contract.Response;
using Refit;

namespace PharmacyService.ApiClient.Clients.IPharmacyApi;

public interface IAuthEndpoint
{
    [Post("/api/Auth/Register")]
    Task<CustomerResponse> Register(
        [Body] CustomerRequest registerRequest);

    [Post("/api/Auth/Login")]
    Task<LoginResponse> Login(
        LoginRequest request,
        CancellationToken cancellationToken);

    [Post("/api/Auth/ReGenerateRefreshToken")]
    Task<LoginResponse> ReGenerateRefreshToken(
        string refreshToken,
        CancellationToken cancellationToken);

    [Post("/api/Auth/ForgotPassword")]
    Task<string> ForgotPassword(
        string email,
        Role role,
        CancellationToken cancellationToken);

    [Post("/api/Auth/ResetPassword")]
    Task<string> ResetPassword(int code,
        string newPassword,
        string email,
        Role role,
        CancellationToken cancellationToken);
}