using Hangfire;
using Pharmacy.Jobs;

namespace Pharmacy.Infrastructure.Extensions;

public static class JobExtensions
{
    public static void AddJob(this WebApplication webApplication)
    {
        var scope = webApplication.Services.CreateScope();
        {
            var recurringJob = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
            recurringJob.AddOrUpdate<CheckExpiredProductsJob>(
                "check-expiry-data-products",
                job => job.CheckExpiredProductsAsync(),
                Cron.MinuteInterval(3));
            recurringJob.AddOrUpdate<Report>(
                "report-to-ceo",
                job => job.ReportToCeo(),
                Cron.Daily(1));
        }
    }
}