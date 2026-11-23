using ProductService.Application.DTOs.Request;
using ProductService.Application.DTOs.Response;
using ProductServiceDomain.Entities;
using ProductServiceDomain.Enums;

namespace ProductService.Application.CQRS.Category.Mapper;

public static class CategoryMappers
{
    public static CategoryEntity ToCategory(
        CreateCategoryRequest request)
    {
        return new CategoryEntity
        {
            Name = request.Name,
            Description = request.Description,
            CategoryStatus = CategoryStatus.Active
        };
    }

    public static void ToCategory(CategoryEntity category,
        UpdateCategoryRequest request)
    {
        category.Name = request.Name;
        category.Description = request.Description;
        category.CategoryStatus = request.CategoryStatus;
    }

    public static CategoryResponse ToCategoryResponse(
        CategoryEntity category)
    {
        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CategoryStatus = category.CategoryStatus
        };
    }

    public static List<CategoryResponse> ToCategoriesResponse(
        List<CategoryEntity> categories)
    {
        return categories
            .Select(ToCategoryResponse)
            .ToList();
    }
}