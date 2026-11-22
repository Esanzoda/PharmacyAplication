using Microsoft.Extensions.DependencyInjection;

namespace ProductService.Application.Extensions;

public static class MediatR
{
    public static void AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(MediatR).Assembly);
        });
    }
}