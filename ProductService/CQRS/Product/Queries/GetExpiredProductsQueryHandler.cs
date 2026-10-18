using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Expired;
using Pharmacy.Domain.Models.Product.DTos.Response;
using ProductService.Interfaces;

namespace ProductService.CQRS.Product.Queries;

public record GetExpiryProductsQuery(
    long PharmacyId,
    PaginationRequest PaginationRequest) : IRequest<List<ExpiredResponse>>;

public class GetExpiredProductsQueryHandler(
    IProductDbContext dbContext) : IRequestHandler<GetExpiryProductsQuery, List<ExpiredResponse>>
{
    public async Task<List<ExpiredResponse>> Handle(
        GetExpiryProductsQuery request,
        CancellationToken cancellationToken)
    {
        var expiredProducts = await dbContext.ExpireDateProducts
            .Where(x => x.PharmacyId == request.PharmacyId)
            .OrderBy(x => x.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
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