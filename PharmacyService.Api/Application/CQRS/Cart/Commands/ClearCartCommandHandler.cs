using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Cart.Mappers;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Api.Application.Exception;

namespace PharmacyService.Api.Application.CQRS.Cart.Commands;

public record ClearCartCommand(
    long CustomerId) : IRequest<CartResponse>;

public class ClearCartCommandHandler(
    IApplicationDbContext dbContext) : IRequestHandler<ClearCartCommand, CartResponse>
{
    public async Task<CartResponse> Handle(
        ClearCartCommand request,
        CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.CustomerEntityId == request.CustomerId,
                cancellationToken);

        if (cart is null)
        {
            throw new ResourceNotFoundException("Cart not found");
        }

        dbContext.CartItems.RemoveRange(cart.CartItems);
        cart.CartItems.Clear();
        cart.TotalAmount = 0;
        await dbContext.SaveChangesAsync(cancellationToken);

        return CartMappers.ToCartResponse(cart);
    }
}