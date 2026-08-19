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

public record LoginEmployeeCommand(
    LoginRequest Request) : IRequest<LoginResponse>;

public class LoginEmployeeCommandHandler(
    IOptionsMonitor<JwtOption> jwt,
    IAuthService auth,
    IApplicationDbContext dbContext,
    IPasswordService passwordService) : IRequestHandler<LoginEmployeeCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginEmployeeCommand request, CancellationToken cancellationToken)
    {
        var dateNow = DateTime.UtcNow;
        var employee = await dbContext.Employees
            .FirstOrDefaultAsync(x => x.Email == request.Request.Email, cancellationToken);

        if (employee is null)
        {
            throw new ResourceNotFoundException("Customer not found");
        }

        var passwordCheck = await passwordService.PasswordVerify(request.Request.Password, employee.PasswordHash);
        if (!passwordCheck)
        {
            throw new BusinessException("Invalid email or password");
        }

        var accessToken = await auth.GenerateTokenForEmployee(employee);

        var newRefreshToken =
            new RefreshToken
            {
                UserId = employee.Id,
                Role = Role.Employee,
                Token = await auth.GenerateRefreshToken(),
                ExpiresAt = dateNow.AddDays(jwt.CurrentValue.RefreshTokenExpiryDay),
                CreatedAt = dateNow
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