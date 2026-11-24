using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Cart.Mappers;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Api.Application.Exception;

namespace PharmacyService.Api.Application.CQRS.Cart.Queries;

public record GetCartByCustomerIdQuery(
    long CustomerId) : IRequest<CartResponse>;

public class GetCartByCustomerIdQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetCartByCustomerIdQuery, CartResponse>
{
    public async Task<CartResponse> Handle(
        GetCartByCustomerIdQuery request,
        CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(x => x.CartItems)
            .ThenInclude(x => x.ProductEntity)
            .FirstOrDefaultAsync(x => x.CustomerEntityId == request.CustomerId,
                cancellationToken);

        return cart is null ? throw new ResourceNotFoundException("Cart not found") : CartMappers.ToCartResponse(cart);
    }
}