using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response.Customer;
using Pharmacy.CQRS.Product.Queries.Customer;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Role.Customer))]
public class ProductController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetAll(int pageNumber, int pageSize)
    {
        var response = await mediator.Send(new GetAllProductsQuery(pageNumber, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetByNameAsync(string name, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetProductsByNameQuery(name, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetByCategoryIdAsync(long categoryId, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetProductsByCategoryIdQuery(categoryId, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetBySalePriceAsync(decimal price, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetProductsBySalePriceQuery(price, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetByCountryAsync(CountryEnum country, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetProductsByCountryQuery(country, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ProductForCustomerResponse>> GetById(long productId)
    {
        var response = await mediator.Send(new GetProductByIdQuery(productId));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> SearchByProductNameFromPharmacy(long pharmacyId,
        string name, int pageNumber, int pageSize)
    {
        var response = await mediator.Send(new GetPharmacyProductsByNameQuery(pharmacyId, name, pageNumber, pageSize));
        return Ok(response);
    }
}