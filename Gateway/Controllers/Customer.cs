using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Cart.DTOs.Request;
using Pharmacy.Domain.Models.Cart.DTOs.Response;
using Pharmacy.Domain.Models.Category.DTOs.Response;
using Pharmacy.Domain.Models.Customer.DTOs.Request;
using Pharmacy.Domain.Models.Customer.DTOs.Response;
using Pharmacy.Domain.Models.Pharmacy.DTOs.Response;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

namespace Gateway.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Role.Customer))]
public class Customer(ICustomerEndpoint customerEndpoint) : ControllerBase
{
    [HttpPatch]
    public async Task<ActionResult<CartItemResponse>> UpdateCartItemQuantity(long productId, int quantity)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await customerEndpoint.UpdateCartItemQuantity(customerId, productId, quantity);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<CustomerResponse>> Update(UpdateCustomerRequest request)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await customerEndpoint.Update(customerId, request);
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<CustomerResponse>> UpdatePassword(string oldPassword, string newPassword)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await customerEndpoint.UpdatePassword(customerId, oldPassword, newPassword);
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<string>> UpdateAddress(string newAddress)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await customerEndpoint.UpdateAddress(customerId, newAddress);
        return Ok(response);
    }


    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetCategoryWithProducts(int categoryId, int page,
        int pageSize)
    {
        var response = await customerEndpoint.GetCategoryWithProducts(categoryId, page, pageSize);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult> AddToCart(CartItemRequest request)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await customerEndpoint.AddToCart(customerId, request);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<CartItemResponse>> RemoveItemFromCart(long productId)
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await customerEndpoint.RemoveItemFromCart(customerId, productId);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<CartResponse>> ClearCart()
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await customerEndpoint.ClearCart(customerId);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<CartResponse>> GetCartByCustomerId()
    {
        var customerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await customerEndpoint.GetCartByCustomerId(customerId);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<PharmacyResponse>>> GetAllPharmacy(int page, int pageSize)
    {
        var response = await customerEndpoint.GetAllPharmacy(page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetAllCategory(int page, int pageSize)
    {
        var response = await customerEndpoint.GetAllCategory(page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetPharmacyProductsByCategory(long pharmacyId,
        long categoryId, int page, int pageSize)
    {
        var response =
            await customerEndpoint.GetPharmacyProductsByCategory(pharmacyId, categoryId, page, pageSize);
        return Ok(response);
    }
}