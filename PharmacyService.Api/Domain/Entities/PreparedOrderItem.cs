using ProductService.Contract.Grpc;

namespace PharmacyService.Api.Domain.Entities;

public class PreparedOrderItem
{
    public ProductGrpcResponse Product { get; set; } = null!;
    public int Quantity { get; set; }
}