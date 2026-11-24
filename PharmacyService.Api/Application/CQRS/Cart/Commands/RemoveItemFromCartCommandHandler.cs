using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Cart.Mappers;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Api.Application.Exception;

namespace PharmacyService.Api.Application.CQRS.Cart.Commands;

public record RemoveItemFromCartCommand(
    long CustomerId,
    long ProductId) : IRequest<CartResponse>;

public class RemoveItemFromCartCommandHandler(
    IApplicationDbContext dbContext) : IRequestHandler<RemoveItemFromCartCommand, CartResponse>
{
    public async Task<CartResponse> Handle(
        RemoveItemFromCartCommand request,
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

        var item = cart.CartItems.FirstOrDefault(x => x.ProductEntityId == request.ProductId);

        if (item is null)
        {
            throw new ResourceNotFoundException("Cart item not found");
        }

        cart.CartItems.Remove(item);
        dbContext.CartItems.Remove(item);

        cart.TotalAmount = cart.CartItems.Sum(x => x.TotalPrice);
        await dbContext.SaveChangesAsync(cancellationToken);

        return CartMappers.ToCartResponse(cart);
    }
}