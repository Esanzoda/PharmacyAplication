using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application;
using PharmacyService.Api.Infrastructure.Data;
using PharmacyService.Api.Setting;

namespace PharmacyService.Api.Extensions;

public static class DbContextExtensions
{
    public static void AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableInterceptor>();
        services.AddScoped<IApplicationDbContext, Api.Infrastructure.AppDbContext>();
        var connectionString = configuration.GetSection(ConnectionStringsOption.SettingName)
            .Get<ConnectionStringsOption>()!;
        services.AddDbContext<Infrastructure.AppDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionString.DefaultConnection)
                .AddInterceptors(sp.GetRequiredService<AuditableInterceptor>());
        });
    }
}