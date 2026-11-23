using FluentValidation;
using ProductService.Application.DTOs.Request;

namespace ProductService.Application.CQRS.Category.Validators;

public class CategoryRequestValidator : AbstractValidator<CreateCategoryRequest>

{
    public CategoryRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage("Name is required");

        RuleFor(request => request.Description)
            .NotEmpty()
            .WithMessage("Description is required");
    }
}