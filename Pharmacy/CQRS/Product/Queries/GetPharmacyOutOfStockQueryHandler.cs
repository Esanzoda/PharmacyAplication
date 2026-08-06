using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries;

public record GetOutOfStockQuery(
    long PharmacyId,
    int Page,
    int PageSize) : IRequest<List<ProductWithBatchResponse>>;

public class GetPharmacyOutOfStockQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetOutOfStockQuery, List<ProductWithBatchResponse>>
{
    public async Task<List<ProductWithBatchResponse>> Handle(GetOutOfStockQuery request,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.Stock == 0)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        if (!product.Any())
            throw new RecourseNotFoundException("Product  not found");

        return mapper.Map<List<ProductWithBatchResponse>>(product);
    }
}