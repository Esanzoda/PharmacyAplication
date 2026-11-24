using Grpc.Contract.Proto;

namespace PharmacyService.Domain.Entities;

public class PreparedOrderItem
{
    public ProductGrpcResponse Product { get; set; } = null!;
    public int Quantity { get; set; }
}