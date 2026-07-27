using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Product.Queries;

public record GetProductsByNameQuery(
    string Name,
    int Page,
    int PageSize) : IRequest<List<ProductResponse>>;

public class GetProductsByNameQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetProductsByNameQuery, List<ProductResponse>>
{
    public async Task<List<ProductResponse>> Handle(GetProductsByNameQuery request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .Where(x => x.Name.ToLower().Contains(request.Name.ToLower()))
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        if (!product.Any())
        {
            throw new RecourseNotFoundException("Product with this naame not found");
        }

        return mapper.Map<List<ProductResponse>>(product);
    }
}