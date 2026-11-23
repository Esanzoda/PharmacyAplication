using Infrastructure.Setting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

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