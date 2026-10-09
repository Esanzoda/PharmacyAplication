using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Product.DTos.Request;
using Pharmacy.Domain.Models.Product.DTos.Response;
using Product.CQRS.Product.Commands;
using Product.CQRS.Product.Queries;

namespace Product.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProductForPharmacy(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ProductForPharmacyResponse>> Add(long pharmacyId,
        [FromBody] ProductRequest request)
    {
        var response = await mediator.Send(new AddProductCommand(pharmacyId, request));
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<ProductForPharmacyResponse>> Update(long pharmacyId,
        long id,
        [FromBody] UpdateProductRequest request
    )
    {
        var responce = await mediator.Send(new UpdateProductCommand(
            pharmacyId, id, request));
        return Ok(responce);
    }

    [HttpDelete]
    public async Task<ActionResult<string>> DeleteById(long pharmacyId, long id)
    {
        var response = await mediator.Send(new DeleteProductCommand(pharmacyId, id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ProductForPharmacyResponse>> GetById(long pharmacyId, long id)
    {
        var response = await mediator.Send(new GetPharmacyProductsByIdQuery(pharmacyId, id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetAll(long pharmacyId,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await mediator.Send(new GetAllPharmacyProductsQuery
            (pharmacyId, paginationRequest));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ProductForPharmacyResponse>> GetByBarcode(long pharmacyId, string barcode)
    {
        var response = await mediator.Send(new GetProductByBarcodeQuery(pharmacyId, barcode));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetByName(long pharmacyId, string name,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await mediator.Send(new GetPharmacyProductsByNameQuery(pharmacyId,
            name, paginationRequest));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetByCategory(long pharmacyId,
        long categoryId,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await mediator.Send(new GetPharmacyProductsByCategoryIdQuery(pharmacyId, categoryId, paginationRequest));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetOutOfStock(long pharmacyId,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await mediator.Send(new GetOutOfStockQuery(
            pharmacyId, paginationRequest));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetLowOfStock(long pharmacyId,
        int minimumQuantity, [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await mediator.Send(new GetLowOfStockQuery(
            pharmacyId, minimumQuantity, paginationRequest));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetByPurchasePrice(long pharmacyId,
        decimal price, [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await mediator.Send(new GetProductsByPurchasePriceQuery(
            pharmacyId, price, paginationRequest));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetByOrderPrice(long pharmacyId,
        decimal price,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await mediator.Send(new GetPharmacyProductsBySalePriceQuery(pharmacyId, price, paginationRequest));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForPharmacyResponse>>> GetByCountry(long pharmacyId,
        CountryEnum country,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response =
            await mediator.Send(new GetPharmacyProductsByCountryQuery(pharmacyId,
                country, paginationRequest));
        return Ok(response);
    }
}