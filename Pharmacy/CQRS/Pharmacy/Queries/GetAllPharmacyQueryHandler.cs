using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Pharmacy.Mapper;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Pharmacy.Queries;

public record GetAllPharmacyQuery(
    int PageNumber,
    int PageSize) : IRequest<List<PharmacyResponse>>;

public class GetAllPharmacyQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetAllPharmacyQuery, List<PharmacyResponse>>
{
    public async Task<List<PharmacyResponse>> Handle(
        GetAllPharmacyQuery request,
        CancellationToken cancellationToken)
    {
        var pharmacies = await dbContext.Pharmacies
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return PharmacyMappers.ToListPharmacyResponse(pharmacies);
    }
}