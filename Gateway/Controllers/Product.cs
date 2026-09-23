using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using Pharmacy.Endpoints.NuGet.Clients.IPharmacyApi;

namespace Gateway.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Role.Customer))]
public class ProductController(IProductEndpoint productEndpoint) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetAll(int pageNumber, int pageSize)
    {
        var response = await productEndpoint.GetAll(pageNumber, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetByNameAsync(string name, int page,
        int pageSize)
    {
        var response = await productEndpoint.GetByName(name, page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetByCategoryIdAsync(long categoryId, int page,
        int pageSize)
    {
        var response = await productEndpoint.GetByCategoryId(categoryId, page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetBySalePriceAsync(decimal price, int page,
        int pageSize)
    {
        var response = await productEndpoint.GetBySalePrice(price, page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetByCountryAsync(CountryEnum country, int page,
        int pageSize)
    {
        var response = await productEndpoint.GetByCountry(country, page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ProductForCustomerResponse>> GetById(long productId)
    {
        var response = await productEndpoint.GetById(productId);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> SearchByProductNameFromPharmacy(long pharmacyId,
        string name, int pageNumber, int pageSize)
    {
        var response = await productEndpoint.SearchByProductNameFromPharmacy(pharmacyId, name, pageNumber, pageSize);
        return Ok(response);
    }
}