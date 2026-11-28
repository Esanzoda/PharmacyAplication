using FluentValidation;
using ProductService.Api.Application.DTOs.Request;

namespace ProductService.Api.Validators;

public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage("Name is required");

        RuleFor(request => request.Description)
            .NotEmpty()
            .WithMessage("Description is required");
    }
}