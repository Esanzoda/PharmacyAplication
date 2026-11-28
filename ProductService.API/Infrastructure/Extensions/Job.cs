using Hangfire;
using ProductService.Api.Infrastructure.Jobs;

namespace ProductService.Api.Infrastructure.Extensions;

public static class Job
{
    public static void AddJob(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var recurringJob = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
        recurringJob.AddOrUpdate<CheckExpiredProductsJob>(
            "check-expiry-data-products",
            job => job.CheckExpiredProductsAsync(),
            Cron.MinuteInterval(1));
    }
}