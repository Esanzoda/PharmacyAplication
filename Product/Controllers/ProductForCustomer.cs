using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using Product.CQRS.Product.Queries.Customer;

namespace Product.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProductForCustomer(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetAll(
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await mediator.Send(new GetAllProductsQuery(paginationRequest));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetByName(
        string name,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await mediator.Send(
            new GetProductsByNameQuery(name, paginationRequest));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetByCategoryId(
        long categoryId,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await mediator.Send(
            new GetProductsByCategoryIdQuery(categoryId, paginationRequest));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetBySalePrice(
        decimal price,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await mediator.Send(
            new GetProductsBySalePriceQuery(price, paginationRequest));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetByCountry(
        CountryEnum country,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await mediator.Send(
            new GetProductsByCountryQuery(country, paginationRequest));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ProductForCustomerResponse>> GetById(
        long productId)
    {
        var response = await mediator.Send(
            new GetProductByIdQuery(productId));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> SearchByProductNameFromPharmacy(
        long pharmacyId,
        string name,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await mediator.Send(
            new GetPharmacyProductsByNameQuery(pharmacyId, name, paginationRequest));
        return Ok(response);
    }
}