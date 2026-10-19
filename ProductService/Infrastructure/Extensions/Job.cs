using Hangfire;
using ProductService.Jobs;

namespace ProductService.Infrastructure.Extensions;

public static class Job
{
    public static void AddJob(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        {
            var recurringJob = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
            recurringJob.AddOrUpdate<CheckExpiredProductsJob>(
                "check-expiry-data-products",
                job => job.CheckExpiredProductsAsync(),
                Cron.MinuteInterval(1));
        }
    }
}