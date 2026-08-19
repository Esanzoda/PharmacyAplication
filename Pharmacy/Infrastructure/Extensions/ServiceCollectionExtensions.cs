using FluentValidation;
using FluentValidation.AspNetCore;
using Pharmacy.Data;
using Pharmacy.Interfaces;
using Pharmacy.Jobs;
using Pharmacy.Services.Auth;
using Pharmacy.Services.DeliveryFee;
using Pharmacy.Services.GoogleMaps;
using Pharmacy.Services.Password;

namespace Pharmacy.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<AuditableInterceptor>();
        serviceCollection.AddAuthorization();
        serviceCollection.AddControllers();
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddFluentValidationAutoValidation();
        serviceCollection.AddValidatorsFromAssemblyContaining<Program>();
        serviceCollection.AddAutoMapper(_ => { }, AppDomain.CurrentDomain.GetAssemblies());
        serviceCollection.AddScoped<IApplicationDbContext, AppDbContext>();
        serviceCollection.AddScoped<CheckExpiredProductsJob>();
        serviceCollection.AddScoped<Report>();
        serviceCollection.AddOpenApi();
        serviceCollection.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); });
        serviceCollection.AddHttpClient<IGeocodingService, GeocodingService>();
        serviceCollection.AddHttpClient<IRoutesService, RoutesApiService>();
        serviceCollection.AddScoped<IPasswordService, PasswordService>();
        serviceCollection.AddScoped<IDeliveryFeeByDistance, DeliveryFeeByDistance>();
        serviceCollection.AddScoped<IAuthService, AuthService>();
    }
}