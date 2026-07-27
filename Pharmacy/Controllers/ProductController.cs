using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Product.Commands;
using Pharmacy.CQRS.Product.Queries;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProductController(IMediator mediator) : ControllerBase
{
    // [Authorize(Roles = nameof(Role.Admin) )]
    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Add([FromBody] ProductRequest request)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new CreateProductCommand(pharmacyId, request));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin) )]
    [HttpPut]
    public async Task<ActionResult<ProductResponse>> Update(long id, [FromBody] ProductRequest request)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new UpdateProductCommand(pharmacyId, id, request));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<ProductResponse>> GetPharmacyById(long id)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetProductByIdQuery(pharmacyId, id));
        return Ok(response);
    }


    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetAllPharmacy(int pageNumber, int pageSize)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetAllPharmacyProductsQuery(pharmacyId, pageNumber, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin) )]
    [HttpDelete]
    public async Task<IActionResult> DeleteProductById(long id)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new DeleteProductCommand(pharmacyId, id));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<ProductResponse>> GetPharmacyByBarcodeAsync(string barcode)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetProductByBarcodeQuery(pharmacyId, barcode));
        return Ok(response);
    }


    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetAllPharmacyProductsByName(string name,
        int page,
        int pageSize)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetPharmacyProductsByNameQuery(pharmacyId, name, page, pageSize));
        return Ok(response);
    }


// [Authorize(Roles = nameof(Role.Admin) )]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetAllPharmacyProductsByCategory(long categoryId,
        int pageNumber, int pageSize)
    {
        var pharmacyId = 1;
        var response =
            await mediator.Send(new GetPharmacyProductsByCategoryIdQuery(pharmacyId, categoryId, pageNumber, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetOutOfStockAsync(int page, int pageSize)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetOutOfStockQuery(pharmacyId, page, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetLowOfStockAsync(int minimumQuantity, int page,
        int pageSize)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetLowOfStockQuery(pharmacyId, minimumQuantity, page, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetByPurchasePriceAsync(decimal price, int page,
        int pageSize)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetProductsByPurchasePriceQuery(pharmacyId, price, page, pageSize));
        return Ok(response);
    }


    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetAllPharmacyProductsByOrderPrice(decimal price,
        int page,
        int pageSize)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetPharmacyProductsByOrderPriceQuery(pharmacyId, price, page, pageSize));
        return Ok(response);
    }


    // [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetAllPharmacyProductsByCountry(CountryEnum country,
        int page,
        int pageSize)
    {
        var pharmacyId = 1;
        var response = await mediator.Send(new GetPharmacyProductsByCountryQuery(pharmacyId, country, page, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer) ]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetAllProduct(int pageNumber, int pageSize)
    {
        var response = await mediator.Send(new GetAllProductsQuery(pageNumber, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetProductsByNameAsync(string name, int page, int pageSize)
    {
        var response = await mediator.Send(new GetProductsByNameQuery(name, page, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetProductsByCategoryIdAsync(long categoryId, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetProductsByCategoryIdQuery(categoryId, page, pageSize));
        return Ok(response);
    }

    // [Authorize(Roles = nameof(Role.Customer))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetByOrderPriseAsync(decimal price, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetProductsByOrderPriceQuery(price, page, pageSize));
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
}