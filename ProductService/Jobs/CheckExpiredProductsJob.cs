using MediatR;
using ProductService.CQRS.Product.Commands;

namespace ProductService.Jobs;

public class CheckExpiredProductsJob(IMediator mediator)
{
    public async Task CheckExpiredProductsAsync()
    {
        await mediator.Send(new CreateExpiredProductsCommand());
    }
}