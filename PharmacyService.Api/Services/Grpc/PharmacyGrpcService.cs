using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application;
using PharmacyService.Api.Application.CQRS.Pharmacy.Mapper;
using PharmacyService.Contract.Grpc;

namespace PharmacyService.Api.Services.Grpc;

public class PharmacyGrpcService(
    IApplicationDbContext dbContext) : PharmacyService.Contract.Grpc.PharmacyService.PharmacyServiceBase
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