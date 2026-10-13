using Grpc.Contract.Proto;

namespace Pharmacy.Domain.Models.Order;

public class PreparedOrderItem
{
    public ProductGrpcResponse Product { get; set; } = null!;
    public int Quantity { get; set; }
}