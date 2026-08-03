using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Product.Queries.Customer;

public record GetProductsBySalePriceQuery(
    decimal Price,
    int Page,
    int PageSize) : IRequest<List<ProductResponse>>;

public class GetProductsByOrderPriseQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetProductsBySalePriceQuery, List<ProductResponse>>
{
    public async Task<List<ProductResponse>> Handle(GetProductsBySalePriceQuery request,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .Where(x => x.SalePrice == request.Price)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        if (!product.Any())
        {
            throw new RecourseNotFoundException("Product with this price  not found");
        }

        return mapper.Map<List<ProductResponse>>(product);
    }
}