using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Product.Queries;

public record GetProductsByOrderPriceQuery(
    decimal Price,
    int Page,
    int PageSize) : IRequest<List<ProductResponse>>;

public class GetProductsByOrderPriseQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetProductsByOrderPriceQuery, List<ProductResponse>>
{
    public async Task<List<ProductResponse>> Handle(GetProductsByOrderPriceQuery request,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .Where(x => x.Price == request.Price)
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