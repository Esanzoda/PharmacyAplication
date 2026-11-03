using FluentValidation;
using Pharmacy.Domain.Models.Category.DTOs.Request;

namespace ProductService.CQRS.Category.Validators;

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