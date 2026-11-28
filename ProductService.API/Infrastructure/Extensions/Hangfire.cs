using Hangfire;
using Hangfire.PostgreSql;
using ProductService.Api.Infrastructure.Setting;

namespace ProductService.Api.Infrastructure.Extensions;

public static class Hangfire
{
    public static void AddHangfire(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetSection(ConnectionStringsOption.SettingName)
            .Get<ConnectionStringsOption>()!;
        services.AddHangfire((_, cfg) =>
            {
                cfg
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UsePostgreSqlStorage(option =>
                    {
                        option.UseNpgsqlConnection(connectionString.DefaultConnection);
                    });
            })
            .AddHangfireServer();
    }
}