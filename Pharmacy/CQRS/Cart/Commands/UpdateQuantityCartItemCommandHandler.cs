using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Cart.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Cart.Commands;

public record UpdateQuantityCartItemCommand(
    long CustomerId,
    long ProductId,
    int Quantity) : IRequest<CartItemResponse>;

public class UpdateQuantityCartItemCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<UpdateQuantityCartItemCommand, CartItemResponse>
{
    public async Task<CartItemResponse> Handle(
        UpdateQuantityCartItemCommand request,
        CancellationToken cancellationToken)
    {
        var cartItem = await dbContext.CartItems
            .Include(x => x.CartEntity)
            .ThenInclude(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.ProductEntityId == request.ProductId &&
                                      x.CartEntity.CustomerEntityId == request.CustomerId,
                cancellationToken);

        if (cartItem is null)
        {
            throw new ResourceNotFoundException("CartItem not found");
        }

        cartItem.Quantity = request.Quantity;
        cartItem.TotalPrice = cartItem.SalePrice * request.Quantity;
        cartItem.CartEntity.TotalAmount = cartItem.CartEntity.CartItems.Sum(x => x.TotalPrice);

        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<CartItemResponse>(cartItem);
    }
}