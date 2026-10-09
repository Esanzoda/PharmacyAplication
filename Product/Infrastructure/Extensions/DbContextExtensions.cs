using Microsoft.EntityFrameworkCore;
using Product.Data;
using Product.Infrastructure.Setting;
using Product.Interfaces;

namespace Product.Infrastructure.Extensions;

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