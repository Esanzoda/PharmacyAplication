using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pharmacy.CQRS.Customer.Models;
using Pharmacy.CQRS.Deliver.Models;
using Pharmacy.CQRS.Employee.Models;
using Pharmacy.Infrastructure.Setting;
using Pharmacy.Models.Domain;

namespace Pharmacy.Services.Auth;

public interface IAuthService
{
    Task<string> GenerateRefreshToken();
    Task<string> GenerateTokenForCustomer(CustomerEntity customer);
    Task<string> GenerateTokenForEmployee(EmployeeEntity employee);
    Task<string> GenerateTokenForDeliver(DeliverEntity deliver);
    Task<string> GenerateTokenForCompanyEmployee(CompanyEmployee companyEmployee);
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


    public Task<string> GenerateTokenForCustomer(CustomerEntity customer)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
            new Claim(ClaimTypes.Email, customer.Email),
            new Claim(ClaimTypes.Role, customer.Role.ToString()),
            new Claim("Latitude", customer.Latitude.ToString()),
            new Claim("Longitude", customer.Longitude.ToString()),
            new Claim(ClaimTypes.StreetAddress, customer.Address)
        };
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

    public Task<string> GenerateTokenForEmployee(EmployeeEntity employee)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
            new Claim(ClaimTypes.Email, employee.Email),
            new Claim(ClaimTypes.Role, employee.Position.ToString()),
            new Claim("PharmacyId", employee.PharmacyId.ToString())
        };
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

    public Task<string> GenerateTokenForDeliver(DeliverEntity deliver)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, deliver.Id.ToString()),
            new Claim(ClaimTypes.Email, deliver.Email),
            new Claim(ClaimTypes.Role, deliver.Role.ToString()),
        };
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

    public Task<string> GenerateTokenForCompanyEmployee(CompanyEmployee companyEmployee)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, companyEmployee.Id.ToString()),
            new Claim(ClaimTypes.Email, companyEmployee.Email),
            new Claim(ClaimTypes.Role, companyEmployee.Role.ToString()),
        };
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