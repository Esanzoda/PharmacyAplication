using Serilog;

namespace PharmacyService.Api.Extensions;

public static class SeriaLogger
{
    public static void AddSeriaLogger(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        builder.Logging.AddSerilog(Log.Logger);
    }
}