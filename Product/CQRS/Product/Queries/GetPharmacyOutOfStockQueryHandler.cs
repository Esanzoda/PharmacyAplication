

using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Product.DTos.Response;
using Product.CQRS.Product.Mappers;
using Product.Interfaces;

namespace Product.CQRS.Product.Queries;

public record GetOutOfStockQuery(
    long PharmacyId,
    PaginationRequest PaginationRequest) : IRequest<List<ProductForPharmacyResponse>>;

public class GetPharmacyOutOfStockQueryHandler(
    IProductDbContext dbContext) : IRequestHandler<GetOutOfStockQuery, List<ProductForPharmacyResponse>>
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
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return ProductMappers.ToListProductForPharmacyResponse(products);
    }
}