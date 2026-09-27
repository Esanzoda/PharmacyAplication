using Serilog;

namespace Pharmacy.Infrastructure.Extensions;

public static class SeriaLoggerExtensions
{
    public static void AddSeriaLogger(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        builder.Logging.AddSerilog(Log.Logger);
    }
}