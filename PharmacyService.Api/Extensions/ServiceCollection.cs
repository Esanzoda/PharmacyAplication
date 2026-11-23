using System.Text.Json.Serialization;
using PharmacyService.Api.Infrastructure.Jobs;
using PharmacyService.Api.Services.Auth;
using PharmacyService.Api.Services.DeliveryFee;
using PharmacyService.Api.Services.GoogleMaps;
using PharmacyService.Api.Services.Password;

namespace PharmacyService.Api.Extensions;

public static class ServiceCollection
{
    public static void AddServiceCollection(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter()));
        services.AddEndpointsApiExplorer();

        services.AddScoped<Report>();
        services.AddHttpClient<IGeocodingService, GeocodingService>();
        services.AddHttpClient<IRoutesService, RoutesApiService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IDeliveryFeeByDistance, DeliveryFeeByDistance>();
        services.AddScoped<IAuthService, AuthService>();
    }
}