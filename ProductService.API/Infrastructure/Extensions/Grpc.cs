namespace ProductService.Api.Infrastructure.Extensions;

public static class Grpc
{
    public static void AddGrpcClient(this IServiceCollection services)
    {
        services.AddGrpcClient < PharmacyService.Contract.Grpc.PharmacyService.PharmacyServiceClient> (option =>
        {
            option.Address = new Uri("https://localhost:7157");
        });
    }
}