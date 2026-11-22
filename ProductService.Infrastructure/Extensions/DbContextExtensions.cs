using Infrastructure.Interceptor;
using Infrastructure.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application;

namespace Infrastructure.Extensions;

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