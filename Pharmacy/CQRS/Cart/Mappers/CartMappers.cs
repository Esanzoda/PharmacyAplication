using Pharmacy.Domain.Models.Cart;
using Pharmacy.Domain.Models.Cart.DTOs.Request;
using Pharmacy.Domain.Models.Cart.DTOs.Response;

namespace Pharmacy.CQRS.Cart.Mappers;

public static class CartMappers
{
    public static CartItemEntity ToCartItem(CartItemRequest request)
    {
        return new CartItemEntity
        {
            ProductEntityId = request.ProductId,
            Quantity = request.Quantity
        };
    }

    public static CartItemResponse ToCartItemResponse(CartItemEntity cartItem)
    {
        return new CartItemResponse
        {
            ProductEntityId = cartItem.ProductEntityId,
            Quantity = cartItem.Quantity,
            SalePrice = cartItem.SalePrice,
            TotalPrice = cartItem.TotalPrice
        };
    }

    public static CartResponse ToCartResponse(CartEntity cartItem)
    {
        return new CartResponse
        {
            Id = cartItem.Id,
            TotalAmount = cartItem.TotalAmount,
            CartItemResponse = cartItem.CartItems
                .Select(ToCartItemResponse)
                .ToList()
        };
    }
}