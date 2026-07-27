using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Cart.Queries;

public record GetCartItemByIQuery(
    long CustomerId) : IRequest<CartResponse>;

public class GetAllCartItemQueryHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<GetCartItemByIQuery, CartResponse>
{
    public async Task<CartResponse> Handle(GetCartItemByIQuery request, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(x => x.CartItems)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId,
                cancellationToken);

        return mapper.Map<CartResponse>(cart);
    }
}