using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Domain.Models.Employee;
using Pharmacy.Infrastructure.Setting;

namespace Pharmacy.Services.Auth;

public interface IAuthService
{
    Task<string> GenerateRefreshToken();
    Task<string> GenerateToken(User user);
}

public class AuthService(IOptionsMonitor<JwtOption> jwt) : IAuthService
{
    public Task<string> GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Task.FromResult(Convert.ToBase64String(randomBytes));
    }


    public Task<string> GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        if (user is EmployeeEntity employee)
        {
            claims.Add(new Claim(ClaimTypes.Role, employee.Position.ToString()));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt.CurrentValue.SecretKey));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: jwt.CurrentValue.Issuer,
            audience: jwt.CurrentValue.Audience,
            claims: claims,
            expires:
            DateTime.UtcNow.AddMinutes(jwt.CurrentValue.AccessTokenExpirationMinutes),
            signingCredentials:
            credentials);

        return Task.FromResult(new JwtSecurityTokenHandler()
            .WriteToken(token));
    }
}