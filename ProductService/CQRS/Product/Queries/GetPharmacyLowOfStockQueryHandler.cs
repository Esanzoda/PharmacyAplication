using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Product.DTos.Response;
using ProductService.CQRS.Product.Mappers;
using ProductService.Interfaces;

namespace ProductService.CQRS.Product.Queries;

public record GetLowOfStockQuery(
    long PharmacyId,
    int MinQuantity,
  PaginationRequest PaginationRequest) : IRequest<List<ProductForPharmacyResponse>>;

public class GetPharmacyLowOfStockQueryHandler(
    IProductDbContext dbContext) : IRequestHandler<GetLowOfStockQuery, List<ProductForPharmacyResponse>>
{
    public async Task<List<ProductForPharmacyResponse>> Handle(
        GetLowOfStockQuery request,
        CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .AsNoTracking()
            .Where(x => x.PharmacyId == request.PharmacyId &&
                        x.Stock <= request.MinQuantity)
            .Include(x => x.ProductBatches)
            .OrderBy(x => x.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return ProductMappers.ToListProductForPharmacyResponse(products);
    }
}