using Microsoft.Extensions.DependencyInjection;
using PharmacyService.ApiClient.Clients.IPharmacyApi;
using Refit;

namespace PharmacyService.ApiClient.Extensions;

public static class ServicesExtension
{
    public static void AddPharmacyApi(
        this IServiceCollection services)
    {
        var baseUrl = "http://localhost:5121";

        services
            .AddRefitClient<IAuthEndpoint>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri(baseUrl));

        services
            .AddRefitClient<ICustomerEndpoint>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri(baseUrl));

        services
            .AddRefitClient<ICustomerForAdmin>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri(baseUrl));

        services
            .AddRefitClient<IDeliver>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri(baseUrl));

        services
            .AddRefitClient<IDeliverForAdmin>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri(baseUrl));

        services
            .AddRefitClient<IEmployeeEndpoint>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri(baseUrl));

        services
            .AddRefitClient<IEmployeeForAdmin>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri(baseUrl));

        services
            .AddRefitClient<IOrderEndpoint>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri(baseUrl));

        services
            .AddRefitClient<IPharmacyEndpoint>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri(baseUrl));

        services
            .AddRefitClient<IPharmacyForAdmin>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri(baseUrl));

        services
            .AddRefitClient<IPurchaseEndpoint>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri(baseUrl));

        services
            .AddRefitClient<ISupplier>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri(baseUrl));
    }
}