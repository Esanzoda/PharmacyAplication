using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries;

public record GetProductsByPurchasePriceQuery(
    long PharmacyId,
    decimal Price,
    int Page,
    int PageSize) : IRequest<List<ProductResponse>>;

public class GetProductsByPurchasePriceQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper)
    : IRequestHandler<GetProductsByPurchasePriceQuery, List<ProductResponse>>
{
    public async Task<List<ProductResponse>> Handle(GetProductsByPurchasePriceQuery request,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.PurchasePrice == request.Price)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        if (!product.Any())
            throw new RecourseNotFoundException("Product with this purchase price  not found");

        return mapper.Map<List<ProductResponse>>(product);
    }
}