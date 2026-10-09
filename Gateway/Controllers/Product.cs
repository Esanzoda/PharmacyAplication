using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using Product.Endpoint.Clients.IProductApi;

namespace Gateway.Controllers;

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
}