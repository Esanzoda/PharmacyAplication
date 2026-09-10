using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Base.Dto.Response;
using Pharmacy.Domain.Models.Customer.DTOs.Request;
using Pharmacy.Domain.Models.Customer.DTOs.Response;
using Refit;

namespace Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

public interface IAuthEndpoint
{
    [Post("api/Auth/Register")]
    Task<CustomerResponse> Register( 
        CustomerRequest registerCommandHandler);
    
    [Post("api/Auth/Login")]
    Task<LoginResponse> Login(
        LoginRequest request,
        CancellationToken cancellationToken);

    [Post("api/Auth/ReGenerateRefreshToken")]
    Task<LoginResponse> ReGenerateRefreshToken(
        string refreshToken,
        CancellationToken cancellationToken);

    [Post("api/Auth/ForgotPassword")]
    Task<LoginResponse> ForgotPassword(
        string email,
        Role role,
        CancellationToken cancellationToken);
    
    [Post("api/Auth/ResetPassword")]
    Task<LoginResponse> ResetPassword(int code,
        string newPassword,
        string email,
        Role role,
        CancellationToken cancellationToken);
    

}