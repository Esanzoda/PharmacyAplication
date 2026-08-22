using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pharmacy.Exception;
using Pharmacy.Infrastructure.Setting;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;
using Pharmacy.Services.Auth;
using Pharmacy.Services.Password;

namespace Pharmacy.CQRS.Auth.Commands;

public record LoginCompanyEmployeeCommand(
    LoginRequest Request) : IRequest<LoginResponse>;

public class LoginCompanyEmployeeHandler(
    IOptionsMonitor<JwtOption> jwt,
    IAuthService auth,
    IApplicationDbContext dbContext,
    IPasswordService passwordService) : IRequestHandler<LoginCompanyEmployeeCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(
        LoginCompanyEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var companyEmployee = await dbContext.CompanyEmployees
            .FirstOrDefaultAsync(x => x.Email == request.Request.Email,
                cancellationToken);
        if (companyEmployee is null)
        {
            throw new ResourceNotFoundException("Company Employee not found");
        }

        var passwordCheck =
            await passwordService.PasswordVerify(request.Request.Password, companyEmployee.PasswordHash);
        if (!passwordCheck)
        {
            throw new BusinessException("Invalid email or password");
        }

        var accessToken = await auth.GenerateTokenForCompanyEmployee(companyEmployee);
        var newRefreshToken =
            new RefreshToken
            {
                UserId = companyEmployee.Id,
                Role = Role.Admin,
                Token = await auth.GenerateRefreshToken(),
                ExpiresAt = now.AddDays(jwt.CurrentValue.RefreshTokenExpiryDay),
                CreatedAt = now
            };

        await dbContext.RefreshTokens
            .AddAsync(newRefreshToken, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Token
        };
    }
}