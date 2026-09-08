using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.Mapper;
using Pharmacy.Domain.Models.Product.DTos.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries;

public record GetOutOfStockQuery(
    long PharmacyId,
    int Page,
    int PageSize) : IRequest<List<ProductForPharmacyResponse>>;

public class GetPharmacyOutOfStockQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetOutOfStockQuery, List<ProductForPharmacyResponse>>
{
    public async Task<List<ProductForPharmacyResponse>> Handle(
        GetOutOfStockQuery request,
        CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .AsNoTracking()
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.Stock == 0)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return ProductMappers.ToListProductForPharmacyResponse(products);
    }
}