using FluentValidation;
using ProductService.Contract.Request;

namespace GatewayService.Api.Validators.Category;

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