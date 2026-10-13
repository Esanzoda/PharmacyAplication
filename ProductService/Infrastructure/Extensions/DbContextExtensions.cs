using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Infrastructure.Setting;
using ProductService.Interfaces;

namespace ProductService.Infrastructure.Extensions;

public static class DbContextExtensions
{
    public static void AddProductDbContext(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddScoped<IProductDbContext, ProductDbContext>();
        service.AddScoped<AuditableInterceptor>();
        var connectionString = configuration.GetSection(ConnectionStringsOption.SettingName)
            .Get<ConnectionStringsOption>()!;
        service.AddDbContext<ProductDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionString.DefaultConnection)
                .AddInterceptors(sp.GetRequiredService<AuditableInterceptor>());
        });
    }
}