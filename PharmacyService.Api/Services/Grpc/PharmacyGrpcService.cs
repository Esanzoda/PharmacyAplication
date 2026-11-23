using Grpc.Contract.Proto;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application;
using PharmacyService.Api.Application.CQRS.Pharmacy.Mapper;
using PharmacyGrpc = Grpc.Contract.Proto.PharmacyService;

namespace PharmacyService.Api.Services.Grpc;

public class PharmacyGrpcService(
    IApplicationDbContext dbContext) : PharmacyGrpc.PharmacyServiceBase
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