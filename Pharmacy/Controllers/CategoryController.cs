using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.CQRS.Category.Commands;
using Pharmacy.CQRS.Category.Queries;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CategoryController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> AddCategory([FromBody] CreateCategoryRequest request)
    {
        var response = await mediator.Send(new CreateCategoryCommand(request));
        return Ok(response);
    }

    [Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpPut]
    public async Task<ActionResult<CategoryResponse>> UpdateCategory(long id, [FromBody] UpdateCategoryRequest request)
    {
        var response = await mediator.Send(new UpdateCategoryCommand(id, request));
        return Ok(response);
    }

    [Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpGet]
    public async Task<ActionResult<CategoryResponse>> GetCategoryById(long id, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
        return Ok(response);
    }

    [Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetAllCategories(int page, int pageSize)
    {
        var response = await mediator.Send(new GetAllCategoriesByPaginationQuery(page, pageSize));
        return Ok(response);
    }

    [Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpDelete]
    public async Task<IActionResult> DeleteCategoryById(long id)
    {
        var response = await mediator.Send(new DeleteCategoryCommand(id));
        return Ok(response);
    }

    [Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetCategoryWithProducts(int categoryId, int page, int pageSize)
    {
        var response = await mediator.Send(new GetCategoryByIdWithProductsQuery(categoryId, page, pageSize));
        return Ok(response);
    }

    [Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetCategoryByName(string name)
    {
        var response = await mediator.Send(new GetCategoryByNameQuery(name));
        return Ok(response);
    }

    [Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetByStatus(CategoryStatus categoryStatus, int pageNumber, int pageSize)
    {
        var response = await mediator.Send(new GetCategoriesByStatusQuery(categoryStatus, pageNumber, pageSize));
        return Ok(response);
    }
}