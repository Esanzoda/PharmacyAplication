using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Category.DTOs.Request;
using Pharmacy.Domain.Models.Category.DTOs.Response;
using Pharmacy.Domain.Models.Product.DTos.Response.Customer;
using Product.CQRS.Category.Commands;
using Product.CQRS.Category.Queries;
using Product.CQRS.Product.Queries.Customer;

namespace Product.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class Category(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> Add([FromBody] CreateCategoryRequest request)
    {
        var response = await mediator.Send(new CreateCategoryCommand(request));
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<CategoryResponse>> Update(long id, [FromBody] UpdateCategoryRequest request)
    {
        var response = await mediator.Send(new UpdateCategoryCommand(id, request));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<CategoryResponse>> GetById(long id,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetAll([FromQuery] PaginationRequest paginationRequest)
    {
        var response = await mediator.Send(
            new GetAllCategoriesByPaginationQuery(paginationRequest));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<string>> DeleteById(long id)
    {
        var response = await mediator.Send(new DeleteCategoryCommand(id));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetProducts(int categoryId,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await mediator.Send(
            new GetProductsByCategoryIdQuery(categoryId, paginationRequest));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetByName(string name)
    {
        var response = await mediator.Send(new GetCategoryByNameQuery(name));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetByStatus(CategoryStatus categoryStatus,
        [FromQuery] PaginationRequest paginationRequest)
    {
        var response = await mediator.Send(
            new GetCategoriesByStatusQuery(categoryStatus, paginationRequest));
        return Ok(response);
    }
}