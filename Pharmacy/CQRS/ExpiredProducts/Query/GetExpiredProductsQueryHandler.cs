using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.ExpiredProducts.Models;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.ExpiredProducts.Query;

public record GetExpiryProductsQuery(
    long PharmacyId,
    int PageNumber,
    int PageSize) : IRequest<List<ExpiredResponse>>;

public class GetExpiredProductsQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetExpiryProductsQuery, List<ExpiredResponse>>
{
    public async Task<List<ExpiredResponse>> Handle(
        GetExpiryProductsQuery request,
        CancellationToken cancellationToken)
    {
        var expiredProducts = await dbContext.ExpireDateProducts
            .Where(x => x.PharmacyId == request.PharmacyId)
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return ToListExpiredResponse(expiredProducts);
    }

    private static List<ExpiredResponse> ToListExpiredResponse(List<ExpiredProductsEntity> products)
    {
        return products
                .Select(ToExpiredResponse)
                .ToList()
            ;
    }

    private static ExpiredResponse ToExpiredResponse(ExpiredProductsEntity product)
    {
        return new ExpiredResponse
        {
            TotalPurchasePrice = product.TotalPurchasePrice,
            TotalSalePrice = product.TotalSalePrice,
            CreatedAt = product.CreatedAt,
            ExpiryDateItemsListResponse = product.ExpiryDateItemsList
                .Select(ToExpiredItemsResponse)
                .ToList()
        };
    }

    private static ExpiredItemsResponse ToExpiredItemsResponse(ExpiredItemsEntity product)
    {
        return new ExpiredItemsResponse
        {
            ExpiryDateEntityId = product.ExpiryDateEntityId,
            ProductBatchId = product.ProductBatchId,
            TotalPurchasePrice = product.TotalPurchasePrice,
            TotalSalePrice = product.TotalSalePrice
        };
    }
}