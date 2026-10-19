using Hangfire;
using Hangfire.PostgreSql;
using ProductService.Infrastructure.Setting;

namespace ProductService.Infrastructure.Extensions;

public static class Hangfire
{
    public static void AddHangfire(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionString = configuration.GetSection(ConnectionStringsOption.SettingName)
            .Get<ConnectionStringsOption>()!;
        serviceCollection.AddHangfire((_, cfg) =>
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