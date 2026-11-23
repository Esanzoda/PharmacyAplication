using MediatR;
using ProductService.Application.CQRS.Product.Commands;

namespace Infrastructure.Jobs;

public class CheckExpiredProductsJob(IMediator mediator)
{
    public async Task CheckExpiredProductsAsync()
    {
        await mediator.Send(new CreateExpiredProductsCommand());
    }
}