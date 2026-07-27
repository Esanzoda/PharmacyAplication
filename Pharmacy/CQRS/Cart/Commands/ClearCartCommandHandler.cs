using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Cart.Commands;

public record ClearCartCommand(
    long CustomerId
) : IRequest<bool>;

public class ClearCartCommandHandler(
    IApplicationDbContext dbContext
) : IRequestHandler<ClearCartCommand, bool>
{
    public async Task<bool> Handle(ClearCartCommand request, CancellationToken cancellationToken)
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
        return true;
    }
}