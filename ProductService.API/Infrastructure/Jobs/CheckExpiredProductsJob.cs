using MediatR;
using ProductService.Api.Application.CQRS.Product.Commands;

namespace ProductService.Api.Infrastructure.Jobs;

public class CheckExpiredProductsJob(IMediator mediator)
{
    public async Task CheckExpiredProductsAsync()
    {
        await mediator.Send(new CreateExpiredProductsCommand());
    }
}