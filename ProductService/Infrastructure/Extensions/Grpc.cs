using Grpc.Contract.Proto;

namespace ProductService.Infrastructure.Extensions;

public static class Grpc
{
    public static void AddGrpcClient(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddGrpcClient<PharmacyService.PharmacyServiceClient>(option =>
        {
            option.Address = new Uri("https://localhost:7166");
        });
    }
}