using ProductService.Api.Infrastructure.Setting;

namespace ProductService.Api.Infrastructure.Extensions;

public static class Redis
{
    public static void AddRedis(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetSection(ConnectionStringsOption.SettingName)
            .Get<ConnectionStringsOption>()!;
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString.Redis;
            options.InstanceName = connectionString.InstanceName;
        });
    }
}