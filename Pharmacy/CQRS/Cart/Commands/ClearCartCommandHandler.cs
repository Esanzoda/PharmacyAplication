using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Cart.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Cart.Commands;

public record ClearCartCommand(
    long CustomerId
) : IRequest<CartResponse>;

public class ClearCartCommandHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<ClearCartCommand, CartResponse>
{
    public async Task<CartResponse> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId, cancellationToken);

        if (cart is null)
        {
            throw new RecourseNotFoundException("Cart not found");
        }

        dbContext.CartItems.RemoveRange(cart.CartItems);
        cart.TotalAmount = 0;
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<CartResponse>(cart);
    }
}