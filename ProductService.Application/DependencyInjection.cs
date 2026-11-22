using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Extensions;

namespace ProductService.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR();
        services.AddFluentValidation();
    }
}