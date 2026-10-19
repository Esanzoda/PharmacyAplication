using Grpc.Contract.Proto;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Pharmacy.Mapper;
using Pharmacy.Interfaces;

namespace Pharmacy.Services.Grpc;

public class PharmacyGrpcService(
    IApplicationDbContext dbContext) : PharmacyService.PharmacyServiceBase
{
    public override async Task<PharmaciesGrpcResponse> GetPharmaciesByIds(
        GetPharmaciesByIdsRequest request, ServerCallContext context)
    {
        var pharmacy = await dbContext.Pharmacies
            .Where(x => request.PharmacyIds.Contains(x.Id))
            .AsNoTracking()
            .ToListAsync(context.CancellationToken);

        return PharmacyMappers.ToListPharmacyGrpcResponse(pharmacy);
    }
}