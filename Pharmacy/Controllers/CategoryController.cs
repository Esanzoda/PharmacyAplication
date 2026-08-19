using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Category.Commands;
using Pharmacy.CQRS.Category.Models.DTOs.Request;
using Pharmacy.CQRS.Category.Models.DTOs.Response;
using Pharmacy.CQRS.Category.Queries;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response.Customer;
using Pharmacy.CQRS.Product.Queries.Customer;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = nameof(Role.Admin))]
public class CategoryController(IMediator mediator) : ControllerBase
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
    public async Task<ActionResult<List<CategoryResponse>>> GetAll(int page, int pageSize)
    {
        var response = await mediator.Send(new GetAllCategoriesByPaginationQuery(page, pageSize));
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var response = await mediator.Send(new DeleteCategoryCommand(id));
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<ProductForCustomerResponse>>> GetProducts(int categoryId, int page,
        int pageSize)
    {
        var response = await mediator.Send(new GetProductsByCategoryIdQuery(categoryId, page, pageSize));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<CategoryResponse>> GetByName(string name)
    {
        var response = await mediator.Send(new GetCategoryByNameQuery(name));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetByStatus(CategoryStatus categoryStatus, int pageNumber,
        int pageSize)
    {
        var response = await mediator.Send(new GetCategoriesByStatusQuery(categoryStatus, pageNumber, pageSize));
        return Ok(response);
    }
}