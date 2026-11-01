using FluentValidation;
using Pharmacy.Domain.Models.Category.DTOs.Request;

namespace Gateway.Validators.Category;

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