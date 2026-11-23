using Grpc.Contract.Proto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Cart.Mappers;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Api.Application.Exception;

namespace PharmacyService.Api.Application.CQRS.Cart.Commands;

public record AddItemToCartCommand(
    long CustomerId,
    CartItemRequest ItemRequest) : IRequest<CartResponse>;

public class AddItemToCartCommandHandler(
    IApplicationDbContext dbContext,
    ProductService.ProductServiceClient productServiceClient) :
    IRequestHandler<AddItemToCartCommand, CartResponse>
{
    public async Task<CartResponse> Handle(
        AddItemToCartCommand request,
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

        var product = await productServiceClient.CheckProductAvailabilityAsync(
            new CheckProductAvailabilityRequest
            {
                Id = request.ItemRequest.ProductId,
                Quantity = request.ItemRequest.Quantity
            });

        if (product == null)
        {
            throw new ResourceNotFoundException("Product not found");
        }

        var existingCartItem = cart.CartItems
            .FirstOrDefault(x => x.ProductEntityId == request.ItemRequest.ProductId);

        if (existingCartItem != null)
        {
            existingCartItem.Quantity += request.ItemRequest.Quantity;
            existingCartItem.TotalPrice = existingCartItem.Quantity * existingCartItem.SalePrice;
        }
        else
        {
            var cartItem = CartMappers.ToCartItem(request.ItemRequest);
            cartItem.CustomerEntityId = request.CustomerId;
            cartItem.ProductEntityId = request.ItemRequest.ProductId;
            cartItem.CartEntity = cart;
            cartItem.SalePrice = (decimal)product.SalePrice;
            cartItem.TotalPrice = cartItem.SalePrice * cartItem.Quantity;

            cart.CartItems.Add(cartItem);
        }

        cart.TotalAmount = cart.CartItems.Sum(x => x.TotalPrice);
        await dbContext.SaveChangesAsync(cancellationToken);

        return CartMappers.ToCartResponse(cart);
    }
}