using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Pharmacy.Infrastructure.Setting;

namespace Pharmacy.Infrastructure.Extensions;

public static class AuthenticationExtensions
{
    public static void AddJwtAuthentication(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var jwt = configuration
            .GetSection(JwtOption.SettingName)
            .Get<JwtOption>()!;
        serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwt.SecretKey))
                };
            });
    }
}