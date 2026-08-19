using FluentValidation;
using Pharmacy.CQRS.Category.Models.DTOs.Request;

namespace Pharmacy.CQRS.Category.CategoryValidators;

public class CategoryRequestValidator : AbstractValidator<CreateCategoryRequest>

{
    public CategoryRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Name is required");

        RuleFor(request => request.Description)
            .NotNull()
            .WithMessage("Description is required");
    }
}