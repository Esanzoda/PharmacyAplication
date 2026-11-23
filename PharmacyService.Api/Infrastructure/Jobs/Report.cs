using MediatR;
using PharmacyService.Api.Application.CQRS.Order.Queries;

namespace PharmacyService.Api.Infrastructure.Jobs;

public class Report(IMediator mediator)
{
    public async Task ReportToCeo()
    {
        await mediator.Send(new ReportQuery());
    }
}