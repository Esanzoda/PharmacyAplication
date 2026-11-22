using Infrastructure.Extensions;
using Infrastructure.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddProductDbContext(configuration);
        services.AddHangfire(configuration);
        services.AddScoped<CheckExpiredProductsJob>();
        services.AddMasstransit(configuration);
        services.AddRedis(configuration);
        services.AddGrpcClient();
        services.AddGrpc();
    }

    public static void AddInfrastructure(this WebApplication app)
    {
        app.AddJob();
    }
}