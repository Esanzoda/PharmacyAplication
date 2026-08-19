using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.ExpiredProducts.Query;

public record GetExpiryProductsQuery(
    long PharmacyId,
    int PageNumber,
    int PageSize) : IRequest<List<ExpireDateProductResponse>>;

public class GetExpiryProductsQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetExpiryProductsQuery, List<ExpireDateProductResponse>>
{
    public async Task<List<ExpireDateProductResponse>> Handle(GetExpiryProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await dbContext.ExpireDateProducts
            .Where(x => x.PharmacyId == request.PharmacyId)
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return mapper.Map<List<ExpireDateProductResponse>>(products);
    }
}