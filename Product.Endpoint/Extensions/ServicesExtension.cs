using Microsoft.Extensions.DependencyInjection;
using Product.Endpoint.Clients.IProductApi;
using Refit;

namespace Product.Endpoint.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddProductApi(
        this IServiceCollection services)
    {
        var baseUrl = "http://localhost:5289";

        services
            .AddRefitClient<ICategory>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri(baseUrl));

        services
            .AddRefitClient<IProductForCustomer>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri(baseUrl));

        services
            .AddRefitClient<IProductForPharmacy>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri(baseUrl));
        return services;
    }
}
