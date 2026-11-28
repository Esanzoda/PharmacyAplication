using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Api.Application.CQRS.Product.Queries.Customer;
using ProductService.Api.Application.DTOs.Request;
using ProductService.Api.Application.DTOs.Response;
using ProductService.Api.Domain.Enums;

namespace ProductService.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProductForCustomerController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetAll(
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await mediator.Send(new GetAllProductsQuery(paginationRequest));
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

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetByIdWithRecommendation(long id,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await mediator.Send(
            new GetProductByIdWithRecommendationQuery(id, paginationRequest));
        return Ok(response);
    }
}