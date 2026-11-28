using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyService.ApiClient.Clients.IPharmacyApi;
using PharmacyService.Contract.Enums;
using PharmacyService.Contract.Request;
using PharmacyService.Contract.Response;
using ProductService.ApiClient.Clients.IProductApi;
using ProductService.Contract.Response;
using ProductPaginationRequest = ProductService.Contract.Request.PaginationRequest;

namespace GatewayService.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Role.Customer))]
public class CustomerController(
    ICustomerEndpoint customerEndpoint,
    ICategory category,
    IProductForCustomer productForCustomer,
    IProductForPharmacy productForPharmacy) : ControllerBase
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
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetCategoryWithProducts(int categoryId,
        [FromQuery] ProductPaginationRequest paginationRequest)
    {
        var response =
            await productForCustomer.GetByCategoryId(categoryId, paginationRequest);
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
    public async Task<ActionResult<List<PharmacyResponse>>> GetAllPharmacy(
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await customerEndpoint.GetAllPharmacy(paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetAllCategory(
        [FromQuery] ProductPaginationRequest paginationRequest)
    {
        var response = await category.GetAll(paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetPharmacyProductsByCategory(long pharmacyId,
        long categoryId,
        [FromQuery] ProductPaginationRequest paginationRequest)
    {
        var response =
            await productForPharmacy.GetByCategory(pharmacyId, categoryId, paginationRequest);
        return Ok(response);
    }
}