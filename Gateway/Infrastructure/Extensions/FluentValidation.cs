using FluentValidation;
using FluentValidation.AspNetCore;

namespace Gateway.Infrastructure.Extensions;

public static class FluentValidation
{
    public static void AddFluentValidation(this IServiceCollection service)
    {
        service.AddFluentValidationAutoValidation();
        service.AddValidatorsFromAssemblyContaining<Program>();
    }
}