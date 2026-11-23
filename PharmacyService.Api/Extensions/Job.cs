using Hangfire;
using PharmacyService.Api.Infrastructure.Jobs;

namespace PharmacyService.Api.Extensions;

public static class Job
{
    public static void AddJob(this WebApplication webApplication)
    {
        var scope = webApplication.Services.CreateScope();
        {
            var recurringJob = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
            recurringJob.AddOrUpdate<Report>(
                "report-to-ceo",
                job => job.ReportToCeo(),
                Cron.Daily(1));
        }
    }
}