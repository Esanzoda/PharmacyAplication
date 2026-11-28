namespace PharmacyService.Api.Extensions;

public static class Grpc
{
    public static void AddGrpcClient(this IServiceCollection services)
    {
        services.AddGrpcClient<ProductService.Contract.Grpc.ProductService.ProductServiceClient>(options =>
        {
            options.Address = new Uri("https://localhost:7064");
        });
    }
}