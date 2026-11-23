using Grpc.Contract.Proto;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class Grpc
{
    public static void AddGrpcClient(this IServiceCollection services)
    {
        services.AddGrpcClient<PharmacyService.PharmacyServiceClient>(option =>
        {
            option.Address = new Uri("https://localhost:7166");
        });
    }
}