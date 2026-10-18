using MediatR;
using Pharmacy.CQRS.Order.Queries;

namespace Pharmacy.Jobs;

public class Report(IMediator mediator)
{
    public async Task ReportToCeo()
    {
        await mediator.Send(new ReportQuery());
    }
}