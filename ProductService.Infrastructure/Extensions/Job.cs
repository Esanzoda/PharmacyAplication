using Hangfire;
using Infrastructure.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class Job
{
    public static void AddJob(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var recurringJob = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
        recurringJob.AddOrUpdate<CheckExpiredProductsJob>(
            "check-expiry-data-products",
            job => job.CheckExpiredProductsAsync(),
            Cron.Daily(1));
    }
}