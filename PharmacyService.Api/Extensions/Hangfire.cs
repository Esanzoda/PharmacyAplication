using Hangfire;
using Hangfire.PostgreSql;
using PharmacyService.Api.Setting;

namespace PharmacyService.Api.Extensions;

public static class Hangfire
{
    public static void AddHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetSection(ConnectionStringsOption.SettingName)
            .Get<ConnectionStringsOption>()!;
        services.AddHangfire((_, cfg) =>
        {
            cfg
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(options => { options.UseNpgsqlConnection(connectionString.DefaultConnection); });
        }).AddHangfireServer();
    }
}