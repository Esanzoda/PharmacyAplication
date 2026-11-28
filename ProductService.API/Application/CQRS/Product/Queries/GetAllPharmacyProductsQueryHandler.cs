using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Api.Application.CQRS.Product.Mappers;
using ProductService.Api.Application.DTOs.Request;
using ProductService.Api.Application.DTOs.Response;

namespace ProductService.Api.Application.CQRS.Product.Queries;

public record GetAllPharmacyProductsQuery(
    long PharmacyId,
    PaginationRequest PaginationRequest) : IRequest<List<ProductForPharmacyResponse>>;

public class GetAllPharmacyProductsQueryHandler(
    IProductDbContext dbContext) :
    IRequestHandler<GetAllPharmacyProductsQuery, List<ProductForPharmacyResponse>>
{
    public async Task<List<ProductForPharmacyResponse>> Handle(
        GetAllPharmacyProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .AsNoTracking()
            .Include(x => x.ProductBatches)
            .Where(x => x.PharmacyId == request.PharmacyId)
            .OrderBy(x => x.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return ProductMappers.ToListProductForPharmacyResponse(products);
    }
}