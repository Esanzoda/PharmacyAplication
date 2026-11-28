using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyService.Contract.Enums;
using ProductService.ApiClient.Clients.IProductApi;
using ProductService.Contract.Request;
using ProductService.Contract.Response;
using CountryEnum = ProductService.Contract.Enums.CountryEnum;

namespace GatewayService.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Role.Customer))]
public class ProductController(IProductForCustomer productEndpoint) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetAll(
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await productEndpoint.GetAll(paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetByNameAsync(
        string name,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await productEndpoint.GetByName(name, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetByCategoryIdAsync(
        long categoryId,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await productEndpoint.GetByCategoryId(categoryId, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetBySalePriceAsync(
        decimal price,
        [FromQuery] PaginationRequest paginationRequest
    )
    {
        var response =
            await productEndpoint.GetBySalePrice(price, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetByCountryAsync(
        CountryEnum country,
        [FromQuery] PaginationRequest paginationRequest
    )
    {
        var response =
            await productEndpoint.GetByCountry(country, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ProductForCustomerResponse>> GetById(
        long productId)
    {
        var response = await productEndpoint.GetById(productId);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> SearchByProductNameFromPharmacy(
        long pharmacyId,
        string name,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await productEndpoint.SearchByProductNameFromPharmacy(pharmacyId, name, paginationRequest);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetByIdWithRecommendation(
        long id,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await productEndpoint.GetByIdWithRecommendation(id, paginationRequest);
        return Ok(response);
    }
}