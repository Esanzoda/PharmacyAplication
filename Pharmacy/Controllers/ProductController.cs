using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.CQRS.Product.Queries.Customer;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProductController(IMediator mediator) : ControllerBase
{
    // [Authorize(Roles = nameof(Role.Customer) ]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetAll(int pageNumber, int pageSize)
    {
        var response = await mediator.Send(new GetAllProductsQuery(pageNumber, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetByNameAsync(string name, int page, int pageSize)
    {
        var response = await mediator.Send(new GetProductsByNameQuery(name, page, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetByCategoryIdAsync(long categoryId, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetProductsByCategoryIdQuery(categoryId, page, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetBySalePriceAsync(decimal price, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetProductsBySalePriceQuery(price, page, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetByCountryAsync(CountryEnum country, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetProductsByCountryQuery(country, page, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetById(long productId)
    {
        var response = await mediator.Send(new GetProductByIdQuery(productId));
        return Ok(response);
    }
}