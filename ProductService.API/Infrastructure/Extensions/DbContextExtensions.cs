using Microsoft.EntityFrameworkCore;
using ProductService.Api.Application;
using ProductService.Api.Infrastructure.Interceptor;
using ProductService.Api.Infrastructure.Setting;

namespace ProductService.Api.Infrastructure.Extensions;

public static class DbContextExtensions
{
    public static void AddProductDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IProductDbContext, ProductDbContext>();
        services.AddScoped<AuditableInterceptor>();
        var connectionString = configuration.GetSection(ConnectionStringsOption.SettingName)
            .Get<ConnectionStringsOption>()!;
        services.AddDbContext<ProductDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionString.DefaultConnection)
                .AddInterceptors(sp.GetRequiredService<AuditableInterceptor>());
        });
    }
}